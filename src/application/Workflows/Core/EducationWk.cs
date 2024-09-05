using application.Utils;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Includes;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Education;
using JsonLocalizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class EducationWk(
    IRepository<EducationModel> educationRepository,
    IGenericCache<EducationModel> genericCache,
    IRepository<ResumeModel> resumeRepository,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortEducationDTO dto)
    {
        try
        {
            var spec = new SortEducationSpec(dto.Skip, dto.Total, dto.ByDesc) { DTO = dto };
            var educations = await genericCache.GetRangeAsync(CachePrefixes.Education + dto.ToString(), () => educationRepository.GetRangeAsync(spec));
            if (educations is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

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
            var education = await genericCache.GetSingleAsync(CachePrefixes.Education + id, () => educationRepository.GetByIdAsync(id));
            if (education is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { education });
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
            var spec = new EducationByIdSpec(id);
            var include = new EducationInclude { IncludeResume = true };
            var education = await educationRepository.GetByIdWithInclude(spec, include);
            if (education is null || education.Resume.UserId != userId)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            await educationRepository.DeleteAsync(education.EducationId);
            return Response(204);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(EducationDTO dto, int resumeId, int userId)
    {
        try
        {
            var resume = await resumeRepository.GetByIdAsync(resumeId);
            if (resume is null || resume.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            var model = mapper.Map<EducationModel>(dto);
            model.ResumeId = resumeId;
            await educationRepository.AddAsync(model);
            return Response(201);
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<EducationDTO> dtos, int resumeId, int userId)
    {
        try
        {
            var resume = await resumeRepository.GetByIdAsync(resumeId);
            if (resume is null || resume.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            var entities = new List<EducationModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<EducationModel>(dto);
                model.ResumeId = resumeId;
                entities.Add(model);
            }

            await educationRepository.AddRangeAsync(entities);
            return Response(201);
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(EducationDTO dto, int experienceId, int userId)
    {
        try
        {
            var spec = new EducationByIdSpec(experienceId);
            var include = new EducationInclude { IncludeResume = true };
            var entity = await educationRepository.GetByIdWithInclude(spec, include);
            if (entity is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            if (entity.Resume is null || entity.Resume.UserId != userId)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            entity = mapper.Map(dto, entity);
            await educationRepository.UpdateAsync(entity);
            return Response(200, new { entity });
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }
}
