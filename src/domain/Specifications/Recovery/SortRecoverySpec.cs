using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Recovery;

public class SortRecoverySpec : SortCollectionSpec<RecoveryModel>
{
    public SortRecoverySpec(int skip, int count, bool byDesc, int userId)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        UserId = userId;

        Query.Where(x => x.UserId.Equals(UserId));

        if (IsExpired.HasValue)
        {
            if (IsExpired.Value)
                Query.Where(x => x.Expires < DateTime.UtcNow);
            else
                Query.Where(x => x.Expires > DateTime.UtcNow);
        }

        Initialize();
    }

    public bool? IsExpired { get; set; }
    public int UserId { get; set; }
}
