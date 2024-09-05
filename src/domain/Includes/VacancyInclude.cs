using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class VacancyInclude : IInclude<VacancyModel>
{
    public Expression<Func<VacancyModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<VacancyModel, object?>>>();

            if (IncludeCompany)
                expressions.Add(x => x.Company);

            if (IncludeResponses)
                expressions.Add(x => x.Responses);

            if (IncludeFavorites)
                expressions.Add(x => x.Favorites);

            return expressions.ToArray();
        }
    }

    public bool IncludeCompany { get; set; } = false;
    public bool IncludeResponses { get; set; } = false;
    public bool IncludeFavorites { get; set; } = false;
}
