using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Vacancy;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class VacancyWk(
    IRepository<VacancyModel> repository,
    IDatabaseTransaction databaseTransaction,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortVacancyDTO dto)
    {
        try
        {
            var spec = new SortVacancySpec(dto.Skip, dto.Total, dto.ByDesc) { DTO = dto, Expressions = [x => x.Company] };
            var vacancies = await repository.GetRangeAsync(spec);
            if (vacancies is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { vacancies });
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
            var spec = new VacancyByIdSpec(id) { Expressions = [x => x.Company] };
            var vacancy = await repository.GetByIdWithInclude(spec);
            if (vacancy is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { vacancy });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveSingle(int id, int companyId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entity = await repository.DeleteAsync(id);
            if (entity is null || entity.CompanyId != companyId)
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

    public async Task<Response> RemoveRange(IEnumerable<int> identifiers, int companyId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entities = await repository.DeleteRangeAsync(identifiers);
            if(entities.Any(e => e is null || e.CompanyId != companyId))
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

    public async Task<Response> AddSingle(VacancyDTO dto)
    {
        try
        {
            var model = mapper.Map<VacancyModel>(dto);
            model.CompanyId = dto.CompanyId;
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<VacancyDTO> dtos)
    {
        try
        {
            var entities = new List<VacancyModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<VacancyModel>(dto);
                model.CompanyId = dto.CompanyId;
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

    public async Task<Response> Update(VacancyDTO dto, int vacancyId, int companyId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(vacancyId);
            if (entity is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (entity.CompanyId != companyId)
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
