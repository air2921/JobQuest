using Ardalis.Specification;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Specifications.Auth;

public class AuthByIdSpec : IncludeSpec<AuthModel>
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
