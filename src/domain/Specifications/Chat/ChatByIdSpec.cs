using Ardalis.Specification;
using domain.Abstractions;
using domain.Models.Chat;
using System.Linq;

namespace domain.Specifications.Chat;

public class ChatByIdSpec : Specification<ChatModel>, IEntityById<ChatModel>
{
    public ChatByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ChatId.Equals(Id));
    }

    public int Id { get; set; }
}
