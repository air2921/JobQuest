using application.Utils;
using AutoMapper;
using common.DTO.ModelDTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Review;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core;

public class ReviewWk(
    IRepository<ReviewModel> repository,
    IGenericCache<ReviewModel> genericCache,
    IDatabaseTransaction databaseTransaction,
    IMapper mapper,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(PaginationDTO dto, int companyId, bool? isRec = null, string? title = null)
    {
        try
        {
            var spec = new SortReviewSpec(dto.Skip, dto.Total, dto.ByDesc, companyId) { IsRecomended = isRec, Title = title };
            var key = $"{CachePrefixes.Review}{dto.ToString()}-{companyId}-{isRec}-{title}";
            var reviews = await genericCache.GetRangeAsync(key, () => repository.GetRangeAsync(spec, [x => x.Company]));
            if (reviews is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { reviews });
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
            var spec = new ReviewByIdSpec(id);
            var review = await genericCache.GetSingleAsync(CachePrefixes.Review + id, 
                () => repository.GetByIdWithInclude(spec, [x => x.Company]));
            if (review is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { review });
        }
        catch (EntityException ex)
        {
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

    public async Task<Response> RemoveSingle(int id, int userId)
    {
        using var transaction = databaseTransaction.Begin();

        try
        {
            var entity = await repository.DeleteAsync(id);
            if (entity is null || entity.UserId != userId)
            {
                transaction.Rollback();
                return Response(403, localizer.Translate(Messages.FORBIDDEN));
            }

            transaction.Commit();
            return Response(204);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(ReviewDTO dto, int userId)
    {
        try
        {
            var model = mapper.Map<ReviewModel>(dto);
            model.UserId =  userId;
            await repository.AddAsync(model);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<ReviewDTO> dtos, int userId)
    {
        try
        {
            var entities = new List<ReviewModel>();

            foreach (var dto in dtos)
            {
                var model = mapper.Map<ReviewModel>(dto);
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

    public async Task<Response> Update(ReviewDTO dto, int reviewId, int userId)
    {
        try
        {
            var review = await repository.GetByIdAsync(reviewId);
            if (review is null || review.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            review = mapper.Map(dto, review);
            await repository.UpdateAsync(review);
            return Response(200, new { review });
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
