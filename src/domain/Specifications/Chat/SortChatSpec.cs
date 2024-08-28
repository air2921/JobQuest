using Ardalis.Specification;
using domain.Models.Chat;
using System.Linq;

namespace domain.Specifications.Chat;

public class SortChatSpec : SortCollectionSpec<ChatModel>
{
    public SortChatSpec(int skip, int count, bool byDesc, int userId) 
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        UserId = userId;

        Query.Where(x => x.CandidateId.Equals(userId) || x.EmployerId.Equals(UserId));

        Query.OrderByDescending(x => x.Messages != null && x.Messages.Any(m => !m.IsRead))
                     .ThenByDescending(x => x.CreatedAt);

        Query.Skip(skip).Take(count);
    }

    public int UserId { get; private set; }
}
