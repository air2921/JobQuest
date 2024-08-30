using application.Utils;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using datahub.Redis;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Company;
using JsonLocalizer;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class CompanyWk(
    IRepository<CompanyModel> repository,
    IDataCache<ConnectionPrimary> dataCache,
    IDatabaseTransaction databaseTransaction,
    ILocalizer localizer,
    IMapper mapper) : Responder
{
    public async Task<Response> GetRange(SortCompanyDTO dto)
    {
        try
        {
            var cache = await dataCache.GetRangeAsync<CompanyModel[]>(CachePrefixes.Company + dto.ToString());
            if (cache is not null)
                return Response(200, new { companies = cache });

            var spec = new SortCompanySpec(dto.Skip, dto.Total, dto.ByDesc) 
            { 
                DTO = dto,
                Expressions = [x => x.Reviews, x => x.Vacancies]
            };

            var companies = await repository.GetRangeAsync(spec);
            if (companies is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            await dataCache.SetAsync(CachePrefixes.Company + dto.ToString(), companies, TimeSpan.FromMinutes(10));
            return Response(200, new { companies });
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
            var cache = await dataCache.GetSingleAsync<CompanyModel>(CachePrefixes.Company + id);
            if (cache is not null)
                return Response(200, new { company = cache });

            var spec = new CompanyByRelationSpec(id) { Expressions = [x => x.Reviews, x => x.Vacancies] };

            var company = await repository.GetRangeAsync(spec);
            if (company is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            await dataCache.SetAsync(CachePrefixes.Company + id, company, TimeSpan.FromMinutes(10));
            return Response(200, new { company });
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
            var company = await repository.DeleteAsync(id);
            if (company is null || company.UserId != userId)
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

    public async Task<Response> AddSingle(CompanyDTO dto, int userId)
    {
        try
        {
            var model = mapper.Map<CompanyModel>(dto);
            model.UserId = userId;
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> Update(CompanyDTO dto, int companyId, int userId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(companyId);
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
