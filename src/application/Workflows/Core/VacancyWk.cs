using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Vacancy;
using JsonLocalizer;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class VacancyWk(
    IRepository<VacancyModel> repository,
    IMapper mapper)
{
    public async Task<Response> GetRange(SortVacancyDTO dto, PaginationDTO pagination)
    {
        try
        {
            var spec = new SortVacancySpec(pagination.Skip, pagination.Total, pagination.ByDesc) { DTO = dto };
            var vacancies = await repository.GetRangeAsync(spec);

            return new Response
            {
                Status = 200,
                ObjectData = new { vacancies }
            };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    public async Task<Response> GetSingle(int id)
    {
        try
        {
            var vacancy = await repository.GetByIdAsync(id);

            return new Response
            {
                Status = 200,
                ObjectData = new { vacancy }
            };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    public async Task<Response> RemoveSingle(int id, int companyId)
    {
        try
        {
            var entity = await repository.DeleteAsync(id);

            return new Response { Status = 204 };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }

    public async Task<Response> AddSingle(VacancyDTO dto)
    {
        try
        {
            var model = mapper.Map<VacancyModel>(dto);
            model.CompanyId = dto.CompanyId;
            await repository.AddAsync(model);
            return new Response { Status = 201 };
        }
        catch (EntityException ex)
        {
            return new Response { Status = 500, Message = ex.Message };
        }
    }
}
