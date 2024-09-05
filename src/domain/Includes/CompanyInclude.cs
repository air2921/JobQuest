using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class CompanyInclude : IInclude<CompanyModel>
{
    public Expression<Func<CompanyModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<CompanyModel, object?>>>();

            if (IncludeUser)
                expressions.Add(x => x.User);

            if (IncludeVacancies)
                expressions.Add(x => x.Vacancies);

            if (IncludeReviews)
                expressions.Add(x => x.Reviews);

            if (IncludeEmployerChats)
                expressions.Add(x => x.EmployerChats);

            if (IncludeSentMessagesAsEmployer)
                expressions.Add(x => x.SentMessagesAsEmployer);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
    public bool IncludeVacancies { get; set; } = false;
    public bool IncludeReviews { get; set; } = false;
    public bool IncludeEmployerChats { get; set; } = false;
    public bool IncludeSentMessagesAsEmployer { get; set; } = false;
}
