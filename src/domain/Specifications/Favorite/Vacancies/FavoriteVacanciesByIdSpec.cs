using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Favorite.Vacancies;

public class FavoriteVacanciesByIdSpec : IncludeSpec<FavoriteVacancyModel>, IEntityById<FavoriteVacancyModel>
{
    public FavoriteVacanciesByIdSpec(int id, int userId)
    {
        Id = id;
        UserId = userId;

        Query.Where(x => x.UserId.Equals(UserId));
        Query.Where(x => x.VacancyId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
    public int UserId { get; set; }
}
