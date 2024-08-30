using Ardalis.Specification;
using domain.Models;
using domain.SpecDTO;
using System;
using System.Linq;

namespace domain.Specifications.Language;

public class SortLanguageSpec : SortCollectionSpec<LanguageModel>
{
    public SortLanguageSpec(int skip, int count, bool byDesc, int userId)
        : base(skip, count, byDesc, x => x.LanguageLevel)
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.Languages is not null && DTO.Languages.Any())
            Query.Where(x => DTO.Languages.Contains(x.Language));

        if (DTO.Levels is not null && DTO.Levels.Any())
            Query.Where(x => DTO.Levels.Contains(x.LanguageLevel));
    }

    public SortLanguageDTO? DTO { get; set; }
}
