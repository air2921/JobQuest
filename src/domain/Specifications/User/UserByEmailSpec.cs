using Ardalis.Specification;
using domain.Models;

namespace domain.Specifications.User;

public class UserByEmailSpec : Specification<UserModel>
{
    public UserByEmailSpec(string email)
    {
        Email = email.ToLowerInvariant();

        Query.Where(x => x.Email.Equals(Email));
    }

    public string Email { get; set; }
}
