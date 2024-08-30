using common.Exceptions;
using domain.Abstractions;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using domain.Specifications.Favorite.Vacancies;
using JsonLocalizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Workflows.Core.Favorites;

public class FavoritesVacancyWk(
    IRepository<FavoriteVacancyModel> repository,
    IDatabaseTransaction databaseTransaction,
    ILocalizer localizer) : Responder
{
    public async Task<Response> GetRange(PaginationDTO dto, int userId)
    {
        try
        {
            var spec = new SortFavoriteVacanciesSpec(dto.Skip, dto.Total, dto.ByDesc, userId);
            var favorites = await repository.GetRangeAsync(spec);
            if (favorites is null)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { favorites });
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
            var favorite = await repository.GetByIdAsync(id);
            if (favorite is null || favorite.UserId != userId)
                return Response(404, localizer.Translate(Messages.NOT_FOUND));

            return Response(200, new { favorite });
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
            var favorite = await repository.DeleteAsync(id);
            if (favorite is null || favorite.UserId != userId)
            {
                transaction.Rollback();
                return Response(404, localizer.Translate(Messages.NOT_FOUND));
            }

            transaction.Commit();
            return Response(200, new { favorite });
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
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddSingle(int resumeId, int userId)
    {
        try
        {
            await repository.AddAsync(new FavoriteVacancyModel
            {
                UserId = userId,
                VacancyId = resumeId
            });
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }

    public async Task<Response> AddRange(IEnumerable<int> identifiers, int userId)
    {
        try
        {
            var list = new List<FavoriteVacancyModel>();

            foreach (var id in identifiers)
                list.Add(new() { UserId = userId, VacancyId = id });

            await repository.AddRangeAsync(list);
            return Response(201);
        }
        catch (EntityException ex)
        {
            return Response(500, ex.Message);
        }
    }
}
