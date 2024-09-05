using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class FavoriteVacancyInclude : IInclude<FavoriteVacancyModel>
{
    public Expression<Func<FavoriteVacancyModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<FavoriteVacancyModel, object?>>>();

            if (IncludeUser)
                expressions.Add(x => x.User);

            if (IncludeVacancy)
                expressions.Add(x => x.Vacancy);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
    public bool IncludeVacancy { get; set; } = false;
}
