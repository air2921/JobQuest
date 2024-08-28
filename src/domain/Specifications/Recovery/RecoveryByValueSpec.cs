using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Recovery;

public class RecoveryByValueSpec : IncludeSpec<RecoveryModel>
{
    public RecoveryByValueSpec(string value)
    {
        Query.Where(x => x.Value.Equals(value));

        if (Expressions is not null)
            IncludeEntities(Expressions);
    }
}
