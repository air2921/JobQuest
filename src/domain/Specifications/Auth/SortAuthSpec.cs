using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Auth;

public class SortAuthSpec : SortCollectionSpec<AuthModel>
{
    public SortAuthSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (UserId.HasValue)
            Query.Where(x => x.UserId.Equals(UserId));

        if (IsExpired.HasValue)
            Query.Where(x => x.IsExpired.Equals(IsExpired));

        Initialize();
    }

    public int? UserId { get; set; }
    public bool? IsExpired { get; set; }
}
