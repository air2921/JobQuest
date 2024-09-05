using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class ExperienceInclude : IInclude<ExperienceModel>
{
    public Expression<Func<ExperienceModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<ExperienceModel, object?>>>();

            if (IncludeResume)
                expressions.Add(x => x.Resume);

            return expressions.ToArray();
        }
    }

    public bool IncludeResume { get; set; } = false;
}
