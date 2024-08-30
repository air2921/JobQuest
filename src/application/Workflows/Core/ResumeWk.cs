using application.Utils;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using datahub.Redis;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Resume;
using JsonLocalizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class ResumeWk(
    IRepository<ResumeModel> repository,
    IDataCache<ConnectionPrimary> dataCache,
    IS3Service s3,
    IDatabaseTransaction databaseTransaction,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortResumeDTO dto)
    {
        try
        {
            var cache = await dataCache.GetRangeAsync<ResumeModel>(CachePrefixes.Resume + dto.ToString());
            if (cache is not null)
                return Response(200, new { resumes = cache });

            var spec = new SortResumeSpec(dto.Skip, dto.Total, dto.ByDesc)
            { 
                DTO = dto,
                Expressions = [x => x.Experiences, x => x.Educations, x => x.User, x => x.User.Languages]  
            }; 
            var resumes = await repository.GetRangeAsync(spec);
            if (resumes is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            await dataCache.SetAsync(CachePrefixes.Resume + dto.ToString(), resumes, TimeSpan.FromMinutes(10));
            return Response(200, new { resumes });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> GetSingle(int id)
    {
        try
        {
            var spec = new ResumeByIdSpec(id) { Expressions = [x => x.Experiences, x => x.Educations, x => x.User, x => x.User!.Languages] };
            var resume = await repository.GetByIdWithInclude(spec);
            if (resume is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            Stream? image = null;
            if (resume.ImageKey is not null)
                image = await s3.Download(resume.ImageKey);

            object obj = image is not null ? new { resume, image } : new { resume }; 
            return Response(200, obj);
        }
        catch (Exception ex) when (ex is EntityException || ex is S3Exception)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveSingle(int id, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entity = await repository.DeleteAsync(id);
            if (entity is null || entity.UserId != userId)
            {
                transaction.Rollback();
                return Response(403, localizer.Translate(Messages.FORBIDDEN));
            }

            if (entity.ImageKey is not null)
                await s3.Delete(entity.ImageKey);

            transaction.Commit();
            return Response(204);
        }
        catch (Exception ex) when (ex is EntityException || ex is S3Exception)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveRange(IEnumerable<int> identifiers, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entities = await repository.DeleteRangeAsync(identifiers);
            if (entities.Any(e => e is null || e.UserId != userId))
            {
                transaction.Rollback();
                return Response(403, localizer.Translate(Messages.FORBIDDEN));
            }

            transaction.Commit();
            return Response(204);
        }
        catch (EntityException ex)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(ResumeDTO dto, int userId, Stream? file = null, string? fileName = null)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var model = mapper.Map<ResumeModel>(dto);
            model.UserId = userId;

            if (file is not null && fileName is not null)
            {
                model.ImageKey = fileName;
                await s3.Upload(file, fileName);
            }

            await repository.AddAsync(model);
            transaction.Commit();
            return Response(201);
        }
        catch (Exception ex) when (ex is EntityException || ex is S3Exception)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<ResumeDTO> dtos, int userId)
    {
        try
        {
            var entities = new List<ResumeModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<ResumeModel>(dto);
                model.UserId = userId;
                entities.Add(model);
            }

            await repository.AddRangeAsync(entities);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(ResumeDTO dto, int resumeId, int userId, Stream? file = null, string? fileName = null)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entity = await repository.GetByIdAsync(resumeId);
            if (entity is null || entity.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (file is not null && fileName is not null)
            {
                if (entity.ImageKey is not null)
                    await s3.Delete(entity.ImageKey);

                entity.ImageKey = Guid.NewGuid().ToString() + fileName;
                await s3.Upload(file, fileName);
            }

            entity = mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            transaction.Commit();
            return Response(200, new { entity });
        }
        catch (Exception ex) when (ex is EntityException || ex is S3Exception)
        {
            transaction.Rollback();
            return Response(500, ex.Message);
        }
    }
}
