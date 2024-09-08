using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Recovery;

public class RecoveryByValueSpec : Specification<RecoveryModel>
{
    public RecoveryByValueSpec(string value)
    {
        Value = value;

        Query.Where(x => x.Value.Equals(value));
    }

    public string Value { get; set; }
}
