using Ardalis.Specification;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Specifications.User;

public class UserByIdSpec : IncludeSpec<UserModel>
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
