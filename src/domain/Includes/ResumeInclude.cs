using domain.Abstractions;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace domain.Includes;

public class ResumeInclude : IInclude<ResumeModel>
{
    public Expression<Func<ResumeModel, object?>>[] Expressions
    {
        get
        {
            var expressions = new List<Expression<Func<ResumeModel, object?>>>();

            if (IncludeUser)
            {
                expressions.Add(x => x.User);
                if (IncludeLanguages)
                    expressions.Add(x => x.User.Languages);
            }

            if (IncludeResponses)
                expressions.Add(x => x.Responses);

            if (IncludeFavorites)
                expressions.Add(x => x.Favorites);

            if (IncludeEducations)
                expressions.Add(x => x.Educations);

            if (IncludeExperiences)
                expressions.Add(x => x.Experiences);

            if (IncludeCandidateChats)
                expressions.Add(x => x.CandidateChats);

            if (IncludeSentMessagesAsCandidate)
                expressions.Add(x => x.SentMessagesAsCandidate);

            return expressions.ToArray();
        }
    }

    public bool IncludeUser { get; set; } = false;
    public bool IncludeLanguages { get; set; } = false;
    public bool IncludeResponses { get; set; } = false;
    public bool IncludeFavorites { get; set; } = false;
    public bool IncludeEducations { get; set; } = false;
    public bool IncludeExperiences { get; set; } = false;
    public bool IncludeCandidateChats { get; set; } = false;
    public bool IncludeSentMessagesAsCandidate { get; set; } = false;
}
