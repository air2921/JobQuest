using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Auth;

public class AuthByValueSpec : IncludeSpec<AuthModel>
{
    public AuthByValueSpec(string value)
    {
        Value = value;

        Query.Where(x => x.Value.Equals(Value));

        if (Expressions is not null)
            IncludeEntities(Expressions);
    }

    public string Value { get; private set; }
}
