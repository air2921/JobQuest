using application.Utils;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Response;
using JsonLocalizer;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class ResponseWk(
    IRepository<ResponseModel> repository,
    IRepository<ResumeModel> resumeRepository,
    IGenericCache<ResponseModel> genericCache,
    IMapper mapper,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRangeAsEmployer(SortResponseDTO dto, int vacancyId, int userId)
    {
        try
        {
            var spec = new SortResponseSpec(dto.Skip, dto.Total, dto.ByDesc, true)
            {
                DTO = dto,
                VacancyId = vacancyId,
                Expressions = [x => x.Vacancy]
            };

            var responses = await genericCache.GetRangeAsync(CachePrefixes.Response_AsEmployer + $"{vacancyId}_" + dto.ToString(),
                () => repository.GetRangeAsync(spec));
            bool antiCondition = responses is null ||
                responses.Any(x => x.Vacancy.Company.UserId != userId);
            if (antiCondition)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { responses });
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> GetRangeAsApplicant(SortResponseDTO dto, int resumeId, int userId)
    {
        try
        {
            var spec = new SortResponseSpec(dto.Skip, dto.Total, dto.ByDesc, false)
            {
                DTO = dto,
                ResumeId = resumeId,
                Expressions = [x => x.Resume]
            };

            var responses = await genericCache.GetRangeAsync(CachePrefixes.Response_AsApplicant + $"{resumeId}_" + dto.ToString(),
                () => repository.GetRangeAsync(spec));
            bool antiCondition = responses is null ||
                responses.Any(x => x.Resume.UserId != userId);
            if (antiCondition)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { responses });
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> GetSingle(int id, int userId)
    {
        try
        {
            var spec = new ResponseByIdSpec(id) { Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy.Company] };
            var response = await genericCache.GetSingleAsync(CachePrefixes.Response + id, () => repository.GetByIdWithInclude(spec));
            var antiCondition = response is null ||
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
            var spec = new ResponseByIdSpec(id) { Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy.Company] };
            var response = await repository.GetByIdWithInclude(spec);
            var antiCondition = response is null ||
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

    public async Task<Response> AddSingle(int resumeId, int vacancyId, int userId)
    {
        try
        {
            var resume = await resumeRepository.GetByIdAsync(resumeId);
            if (resume is null || resume.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            var model = new ResponseModel
            {
                ResumeId = resumeId,
                VacancyId = vacancyId
            };
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
            var spec = new ResponseByIdSpec(responseId) { Expressions = [x => x.Resume, x => x.Vacancy, x => x.Vacancy.Company] };
            var response = await repository.GetByIdWithInclude(spec);
            if (response is null || response.Vacancy.Company.CompanyId != companyId)
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
