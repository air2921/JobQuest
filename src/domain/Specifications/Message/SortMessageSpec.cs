using Ardalis.Specification;
using domain.Models.Chat;
using System;
using System.Linq;

namespace domain.Specifications.Message;

public class SortMessageSpec : SortCollectionSpec<MessageModel>
{
    public SortMessageSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.SentAt)
    {
        if (OnlyRead.HasValue)
            Query.Where(x => x.IsRead.Equals(OnlyRead));

        if (!string.IsNullOrWhiteSpace(KeyWord))
        {
            Query.Where(x => x.Message.Contains(KeyWord, StringComparison.InvariantCultureIgnoreCase));
            Query.OrderBy(x => Math.Abs(x.Message.IndexOf(KeyWord) - x.Message.Length));
        }

        Initialize();
    }

    public bool? OnlyRead { get; set; }
    public string? KeyWord { get; set; }
}
