using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Language;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class LanguageWk(
    IRepository<LanguageModel> repository,
    IDatabaseTransaction databaseTransaction,
    IMapper mapper,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(SortLanguageDTO dto, int userId)
    {
        try
        {
            var spec = new SortLanguageSpec(dto.Skip, dto.Total, dto.ByDesc, userId) { DTO = dto };
            var languages = await repository.GetRangeAsync(spec);
            if (languages is null)
                return Response(400, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { languages });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> GetSingle(int id, int userId)
    {
        try
        {
            var language = await repository.GetByIdAsync(id);
            if (language is null || language.UserId != userId)
                return Response(400, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { language });
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
            var entity = await repository.DeleteAsync(id);
            if (entity is null || entity.UserId != userId)
            {
                transaction.Rollback();
                return Response(404, localizer.Translate(Messages.NOT_FOUND));
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

    public async Task<Response> AddSingle(LanguageDTO dto, int userId)
    {
        try
        {
            var model = mapper.Map<LanguageModel>(dto);
            model.UserId = userId;
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<LanguageDTO> dtos, int userId)
    {
        try
        {
            var entities = new List<LanguageModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<LanguageModel>(dto);
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

    public async Task<Response> Update(LanguageDTO dto, int languageId, int userId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(languageId);
            if (entity is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (entity.UserId != userId)
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
