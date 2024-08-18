using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.User;

public class SortUserSpec : SortCollectionSpec<UserModel>
{
    public SortUserSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (Roles is not null && Roles.Length > 0)
            Query.Where(x => Roles.Contains(x.Role));

        if (IsBlocked.HasValue)
            Query.Where(x => x.IsBlocked.Equals(IsBlocked.Value));

        Initialize();
    }

    public string[]? Roles { get; set; }
    public bool? IsBlocked { get; set; }
}
