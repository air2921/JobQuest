using Ardalis.Specification;
using domain.SpecDTO;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Language;

public class SortLanguageSpec : SortCollectionSpec<LanguageModel>
{
    public SortLanguageSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.Level)
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.ResumeId.HasValue)
            Query.Where(x => x.ResumeId.Equals(DTO.ResumeId));

        if (DTO.Level.HasValue)
            Query.Where(x => x.Level >= DTO.Level);

        Initialize();
    }

    public SortLanguageDTO? DTO { get; set; }
}
