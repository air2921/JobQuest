using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Auth;

public class CountAuthSpec : Specification<AuthModel>
{
    public CountAuthSpec()
    {
        if (UserId.HasValue)
            Query.Where(x => x.UserId.Equals(UserId));

        if (IsExpired.HasValue)
            Query.Where(x => x.IsExpired.Equals(IsExpired));
    }

    public bool? IsExpired { get; set; }
    public int? UserId { get; set; }
}
