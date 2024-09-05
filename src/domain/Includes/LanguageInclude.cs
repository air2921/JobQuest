using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class LanguageInclude : IInclude<LanguageModel>
{
    public Expression<Func<LanguageModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<LanguageModel, object?>>>();

            if (IncludeUser)
                expressions.Add(x => x.User);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
}
