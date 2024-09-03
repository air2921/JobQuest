using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Auth;

public class AuthByIdSpec : Specification<AuthModel>, IEntityById<AuthModel>
{
    public AuthByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.TokenId.Equals(Id));
    }

    public int Id { get; set; }
}
