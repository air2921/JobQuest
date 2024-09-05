using domain.Abstractions;
using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class ChatInclude : IInclude<ChatModel>
{
    public Expression<Func<ChatModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<ChatModel, object?>>>();

            if (IncludeCandidateUser)
                expressions.Add(x => x.CandidateUser);

            if (IncludeEmployerUser)
                expressions.Add(x => x.EmployerUser);

            if (IncludeMessages)
                expressions.Add(x => x.Messages);

            return expressions.ToArray();
        }
    }

    public bool IncludeCandidateUser { get; set; } = false;
    public bool IncludeEmployerUser { get; set; } = false;
    public bool IncludeMessages { get; set; } = false;
}
