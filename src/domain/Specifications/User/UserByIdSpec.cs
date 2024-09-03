using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.User;

public class UserByIdSpec : Specification<UserModel>, IEntityById<UserModel>
{
    public UserByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.UserId.Equals(Id));
    }

    public int Id { get; set; }
}
