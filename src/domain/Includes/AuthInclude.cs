using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class AuthInclude : IInclude<AuthModel>
{
    public Expression<Func<AuthModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<AuthModel, object?>>>();

            if (IncludeUser)
                expressions.Add(x => x.User);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
}
