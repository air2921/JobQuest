using application.Utils;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Experience;
using JsonLocalizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class ExperienceWk(
    IRepository<ExperienceModel> experienceRepository,
    IRepository<ResumeModel> resumeRepository,
    IGenericCache<ExperienceModel> genericCache,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortExperienceDTO dto)
    {
        try
        {
            var spec = new SortExperienceSpec(dto.Skip, dto.Total, dto.ByDesc) { DTO = dto };
            var experiences = await genericCache.GetRangeAsync(CachePrefixes.Experience + dto.ToString(), () => experienceRepository.GetRangeAsync(spec));
            if (experiences is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { experiences });
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
            var experience = await genericCache.GetSingleAsync(CachePrefixes.Experience + id, () => experienceRepository.GetByIdAsync(id));
            if (experience is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { experience });
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
            var spec = new ExperienceByIdSpec(id) { Expressions = [x => x.Resume] };
            var experience = await experienceRepository.GetByIdWithInclude(spec);
            if (experience is null || experience.Resume.UserId != userId)
                return Response(403, localizer.Translate(Messages.FORBIDDEN));

            await experienceRepository.DeleteAsync(experience.ExperienceId);
            return Response(204);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(ExperienceDTO dto, int resumeId, int userId)
    {
        try
        {
            var resume = await resumeRepository.GetByIdAsync(resumeId);
            if (resume is null || resume.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            var model = mapper.Map<ExperienceModel>(dto);
            model.ResumeId = resumeId;
            await experienceRepository.AddAsync(model);
            return Response(201);
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<ExperienceDTO> dtos, int resumeId, int userId)
    {
        try
        {
            var resume = await resumeRepository.GetByIdAsync(resumeId);
            if (resume is null || resume.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            var entities = new List<ExperienceModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<ExperienceModel>(dto);
                model.ResumeId = resumeId;
                entities.Add(model);
            }

            await experienceRepository.AddRangeAsync(entities);
            return Response(201);
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(ExperienceDTO dto, int experienceId, int userId)
    {
        try
        {
            var spec = new ExperienceByIdSpec(experienceId) { Expressions = [x => x.Resume] };
            var entity = await experienceRepository.GetByIdWithInclude(spec);
            if (entity is null || entity.Resume.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            entity = mapper.Map(dto, entity);
            await experienceRepository.UpdateAsync(entity);
            return Response(200, new { entity });
        }
        catch (Exception ex) when (ex is EntityException || ex is ValidationException)
        {
            return Response(500, ex.Message);
        }
    }
}
