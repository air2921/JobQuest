using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Favorite.Resumes;

public class FavoriteResumesByIdSpec : IncludeSpec<FavoriteResumeModel>, IEntityById<FavoriteResumeModel>
{
    public FavoriteResumesByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ResumeId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}