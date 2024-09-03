using application.Utils;
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
    IRepository<VacancyModel> vacancyRepository,
    IRepository<CompanyModel> companyRepository,
    IGenericCache<VacancyModel> genericCache,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortVacancyDTO dto)
    {
        try
        {
            var spec = new SortVacancySpec(dto.Skip, dto.Total, dto.ByDesc) { DTO = dto };
            var vacancies = await genericCache.GetRangeAsync(CachePrefixes.Vacancy + dto.ToString(), 
                () => vacancyRepository.GetRangeAsync(spec, [x => x.Company]));
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
            var spec = new VacancyByIdSpec(id);
            var vacancy = await genericCache.GetSingleAsync(CachePrefixes.Vacancy + id, 
                () => vacancyRepository.GetByIdWithInclude(spec, [x => x.Company]));
            if (vacancy is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { vacancy });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveSingle(int id, int companyId, int userId)
    {
        try
        {
            var spec = new VacancyByIdSpec(id);
            var entity = await vacancyRepository.GetByIdWithInclude(spec, [x => x.Company]);
            if (entity is null || entity.CompanyId != companyId || entity.Company.UserId != userId)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            await vacancyRepository.DeleteAsync(id);
            return Response(204);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(VacancyDTO dto, int companyId, int userId)
    {
        try
        {
            var entity = await companyRepository.GetByIdAsync(companyId);
            if (entity is null || entity.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            var model = mapper.Map<VacancyModel>(dto);
            model.CompanyId = companyId;
            await vacancyRepository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<VacancyDTO> dtos, int companyId, int userId)
    {
        var entities = new List<VacancyModel>();

        try
        {
            var entity = await companyRepository.GetByIdAsync(companyId);
            if (entity is null || entity.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            foreach (var dto in dtos)
            {
                var model = mapper.Map<VacancyModel>(dto);
                model.CompanyId = companyId;
                entities.Add(model);
            }

            await vacancyRepository.AddRangeAsync(entities);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(VacancyDTO dto, int vacancyId, int companyId, int userId)
    {
        var spec = new VacancyByIdSpec(vacancyId);
        var entity = await vacancyRepository.GetByIdWithInclude(spec, [x => x.Company]);

        try
        {
            if (entity is null || entity.CompanyId != companyId || entity.Company.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            entity = mapper.Map(dto, entity);
            await vacancyRepository.UpdateAsync(entity);
            return Response(200, new { entity });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
