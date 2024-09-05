using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class ResponseInclude : IInclude<ResponseModel>
{
    public Expression<Func<ResponseModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<ResponseModel, object?>>>();

            if (IncludeResume)
                expressions.Add(x => x.Resume);

            if (IncludeVacancy)
            {
                expressions.Add(x => x.Vacancy);
                if (IncludeCompany)
                    expressions.Add(x => x.Vacancy.Company);
            }

            return expressions.ToArray();
        }
    }

    public bool IncludeResume { get; set; } = false;
    public bool IncludeCompany { get; set; } = false;
    public bool IncludeVacancy { get; set; } = false;
}
