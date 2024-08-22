using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Language;

public class SortLanguageSpec : SortCollectionSpec<LanguageModel>
{
    public SortLanguageSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.Level)
    {
        if (ResumeId.HasValue)
            Query.Where(x => x.ResumeId.Equals(ResumeId));

        if (Level.HasValue)
            Query.Where(x => x.Level >= Level);

        Initialize();
    }

    public int? ResumeId { get; set; }
    public int? Level { get; set; }
}
