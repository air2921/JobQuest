using Ardalis.Specification;
using domain.Abstractions;
using domain.Models.Chat;
using System.Linq;

namespace domain.Specifications.Message;

public class MessageByIdSpec : IncludeSpec<MessageModel>, IEntityById<MessageModel>
{
    public MessageByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.MessageId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
