using Ardalis.Specification;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Response;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class ResponseWk(
    IRepository<ResponseModel> repository,
    IMapper mapper,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(SortResponseDTO dto, int resumeId, int userId)
    {
        try
        {
            var spec = new SortResponseSpec(dto.Skip, dto.Total, dto.ByDesc, resumeId)
            { 
                DTO = dto,
                Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy!.Company]
            };

            var responses = await repository.GetRangeAsync(spec);
            bool antiCondition = responses is null ||
                responses.Any(x => x.Resume is null ||
                x.Vacancy is null || x.Vacancy.Company is null ||
                (x.Vacancy.Company.UserId != userId && x.Resume.UserId != userId));
            if (antiCondition)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { responses });
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
            var spec = new ResponseByIdSpec(id) { Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy!.Company] };
            var response = await repository.GetByIdWithInclude(spec);
            var antiCondition = response is null || response.Resume is null ||
                response.Resume.User is null || response.Vacancy is null || response.Vacancy.Company is null ||
                (response.Vacancy.Company.UserId != userId && response.Resume.UserId != userId);
            if (antiCondition)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { response });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> RemoveSingle(int id, int userId)
    {
        try
        {
            var spec = new ResponseByIdSpec(id) { Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy!.Company] };
            var response = await repository.GetByIdWithInclude(spec);
            var antiCondition = response is null || response.Resume is null ||
                response.Resume.User is null || response.Vacancy is null || response.Vacancy.Company is null ||
                (response.Vacancy.Company.UserId != userId && response.Resume.UserId != userId);
            if (antiCondition)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            await repository.DeleteAsync(id);
            return Response(204);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<ResponseDTO> dtos)
    {
        try
        {
            var entities = new List<ResponseModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<ResponseModel>(dto);
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

    public async Task<Response> AddSingle(ResponseDTO dto)
    {
        try
        {
            var model = mapper.Map<ResponseModel>(dto);
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(ResponseDTO dto, int responseId, int companyId)
    {
        try
        {
            var spec = new ResponseByIdSpec(responseId) { Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy!.Company] };
            var response = await repository.GetByIdWithInclude(spec);
            var antiCondition = response is null || response.Resume is null ||
                response.Vacancy is null || response.Vacancy.Company is null ||
                response.Vacancy.Company.UserId != companyId;
            if (antiCondition)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            response = mapper.Map(dto, response!);
            await repository.UpdateAsync(response);
            return Response(200, new { response });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
