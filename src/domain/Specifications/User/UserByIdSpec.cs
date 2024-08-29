using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.User;

public class UserByIdSpec : IncludeSpec<UserModel>, IEntityById<UserModel>
{
    public UserByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.UserId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
