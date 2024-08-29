using Ardalis.Specification;
using domain.Models.Chat;
using System;
using System.Linq;

namespace domain.Specifications.Chat;

public class ChatByIdSpec : IncludeSpec<ChatModel>
{
    public ChatByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ChatId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
