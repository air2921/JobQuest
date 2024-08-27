using Ardalis.Specification;
using domain.Models.Chat;
using System.Linq;

namespace domain.Specifications.Chat;

public class SortChatSpec : SortCollectionSpec<ChatModel>
{
    public SortChatSpec(int skip, int count, bool byDesc) 
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        Query.OrderByDescending(x => x.Messages != null && x.Messages.Any(m => !m.IsRead))
                     .ThenByDescending(x => x.CreatedAt);

        Query.Skip(skip).Take(count);
    }
}
