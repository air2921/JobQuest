using domain.Abstractions;
using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class MessageInclude : IInclude<MessageModel>
{
    public Expression<Func<MessageModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<MessageModel, object?>>>();

            if (IncludeChat)
                expressions.Add(x => x.Chat);

            if (IncludeEmployer)
                expressions.Add(x => x.Employer);

            if (IncludeCandidate)
                expressions.Add(x => x.Candidate);

            return expressions.ToArray();
        }
    }

    public bool IncludeChat { get; set; } = false;
    public bool IncludeEmployer { get; set; } = false;
    public bool IncludeCandidate { get; set; } = false;
}
