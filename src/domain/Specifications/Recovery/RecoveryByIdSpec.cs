using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Recovery;

public class RecoveryByIdSpec : IncludeSpec<RecoveryModel>
{
    public RecoveryByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.TokenId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
