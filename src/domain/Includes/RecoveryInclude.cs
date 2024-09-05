using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class RecoveryInclude : IInclude<RecoveryModel>
{

    public Expression<Func<RecoveryModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<RecoveryModel, object?>>>();

            if (IncludeUser)
                expressions.Add(x => x.User);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
}
