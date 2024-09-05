using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class ReviewInclude : IInclude<ReviewModel>
{
    public Expression<Func<ReviewModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<ReviewModel, object?>>>();

            if (IncludeCompany)
                expressions.Add(x => x.Company);

            if (IncludeUser)
                expressions.Add(x => x.User);

            return expressions.ToArray();
        }
    }

    public bool IncludeCompany { get; set; } = false;
    public bool IncludeUser { get; set; } = false;
}
