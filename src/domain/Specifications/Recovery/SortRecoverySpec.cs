using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Recovery;

public class SortRecoverySpec : SortCollectionSpec<RecoveryModel>
{
    public SortRecoverySpec(int skip, int count, bool byDesc, bool isUsed)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        IsUsed = isUsed;

        if (UserId.HasValue)
            Query.Where(x => x.UserId.Equals(UserId.Value));

        Query.Where(x => x.IsUsed.Equals(IsUsed));

        if (IsExpired.HasValue)
            Query.Where(x => x.IsExpired.Equals(IsExpired));

        Initialize();
    }

    public bool? IsExpired { get; set; }
    public int? UserId { get; set; }
    public bool IsUsed { get; set; }
}
