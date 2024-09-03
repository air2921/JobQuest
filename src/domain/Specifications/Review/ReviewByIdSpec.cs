using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Review;

public class ReviewByIdSpec : Specification<ReviewModel>, IEntityById<ReviewModel>
{
    public ReviewByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ReviewId.Equals(Id));
    }

    public int Id { get; set; }
}