using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class UserInclude : IInclude<UserModel>
{
    public Expression<Func<UserModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<UserModel, object?>>>();

            if (IncludeCompany)
                expressions.Add(x => x.Company);

            if (IncludeReviews)
                expressions.Add(x => x.Reviews);

            if (IncludeResumes)
                expressions.Add(x => x.Resumes);

            if (IncludeLanguages)
                expressions.Add(x => x.Languages);

            if (IncludeFavoriteResumes)
                expressions.Add(x => x.FavoriteResumes);

            if (IncludeFavoriteVacancies)
                expressions.Add(x => x.FavoriteVacancies);

            if (IncludeAuths)
                expressions.Add(x => x.Auths);

            if (IncludeRecoveries)
                expressions.Add(x => x.Recoveries);

            return expressions.ToArray();
        }
    }

    public bool IncludeCompany { get; set; } = false;
    public bool IncludeReviews { get; set; } = false;
    public bool IncludeResumes { get; set; } = false;
    public bool IncludeLanguages { get; set; } = false;
    public bool IncludeFavoriteResumes { get; set; } = false;
    public bool IncludeFavoriteVacancies { get; set; } = false;
    public bool IncludeAuths { get; set; } = false;
    public bool IncludeRecoveries { get; set; } = false;
}
