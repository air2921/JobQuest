using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Review;

public class ReviewByIdSpec : IncludeSpec<ReviewModel>, IEntityById<ReviewModel>
{
    public ReviewByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ReviewId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}