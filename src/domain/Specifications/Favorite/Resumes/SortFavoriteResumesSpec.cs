using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Favorite.Resumes;

public class SortFavoriteResumesSpec : SortCollectionSpec<FavoriteResumeModel>
{
    public SortFavoriteResumesSpec(int skip, int count, bool byDesc, int userId)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        UserId = userId;

        Query.Where(x => x.UserId.Equals(UserId));

        Initialize();
    }

    public int UserId { get; set; }
}
