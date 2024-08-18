using Ardalis.Specification;
using domain.Models;

namespace domain.Specifications.Favorite;

public class SortFavoriteSpec : SortCollectionSpec<FavoriteModel>
{
    public SortFavoriteSpec(int skip, int count, bool byDesc, int userId)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        UserId = userId;

        Query.Where(x => x.UserId.Equals(UserId));

        Initialize();
    }

    public int UserId { get; set; }
}
