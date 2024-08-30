using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Education;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class EducationWk(
    IRepository<EducationModel> repository,
    IDatabaseTransaction databaseTransaction,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortEducationDTO dto)
    {
        try
        {
            var spec = new SortEducationSpec(dto.Skip, dto.Total, dto.ByDesc) { DTO = dto };
            var educations = await repository.GetRangeAsync(spec);
            if (educations is null)
                return Response(400, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { educations });
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
            var education = await repository.GetByIdAsync(id);
            if (education is null)
                return Response(400, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { education });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveSingle(int id, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var experience = await repository.DeleteAsync(id);
            if (experience is null || experience.Resume is null || experience.Resume.User is null)
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

    public async Task<Response> RemoveRange(IEnumerable<int> identifiers, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entities = await repository.DeleteRangeAsync(identifiers);
            if (entities.Any(e => e is null || e.Resume is null || e.Resume.User is null || e.Resume.User.UserId != userId))
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

    public async Task<Response> AddSingle(EducationDTO dto)
    {
        try
        {
            var model = mapper.Map<EducationModel>(dto);
            model.ResumeId = dto.ResumeId;
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<EducationDTO> dtos)
    {
        try
        {
            var entities = new List<EducationModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<EducationModel>(dto);
                model.ResumeId = dto.ResumeId;
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

    public async Task<Response> Update(EducationDTO dto, int experienceId, int userId)
    {
        try
        {
            var spec = new EducationByIdSpec(experienceId) { Expressions = [x => x.Resume, x => x.Resume!.User] };
            var entity = await repository.GetByIdWithInclude(spec);
            if (entity is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (entity.Resume is null || entity.Resume.User is null || entity.Resume.User is null || entity.Resume.User.UserId != userId)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            entity = mapper.Map(dto, entity);
            await repository.UpdateAsync(entity);
            return Response(200, new { entity });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
