using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class FavoriteResumeInclude : IInclude<FavoriteResumeModel>
{
    public Expression<Func<FavoriteResumeModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<FavoriteResumeModel, object?>>>();

            if (IncludeUser)
                expressions.Add(x => x.User);

            if (IncludeResume)
                expressions.Add(x => x.Resume);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
    public bool IncludeResume { get; set; } = false;
}
