using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Auth;

public class AuthByIdSpec : IncludeSpec<AuthModel>, IEntityById<AuthModel>
{
    public AuthByIdSpec(int id)
    {
        Id = id;

        Query.Where(x => x.TokenId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
