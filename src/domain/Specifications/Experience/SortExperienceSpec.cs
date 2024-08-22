using Ardalis.Specification;
using domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace domain.Specifications.Experience;

public class SortExperienceSpec : SortCollectionSpec<ExperienceModel>
{
    public SortExperienceSpec(int skip, int count, bool byDesc)
    : base(skip, count, byDesc, x => x.ExperienceCountInMounts) 
    {
        if (ResumeId.HasValue)
            Query.Where(x => x.ResumeId.Equals(ResumeId));

        if (SpecialityNames is not null && SpecialityNames.Any())
            Query.Where(x => SpecialityNames.Contains(x.SpecialityName));

        if (Companies is not null && Companies.Any())
            Query.Where(x => Companies.Contains(x.Company));

        if (Locations is not null && Locations.Any())
            Query.Where(x => Locations.Contains(x.Location));

        if (HasWebSite.HasValue)
            Query.Where(x => HasWebSite.Value
                ? !string.IsNullOrWhiteSpace(x.WebSite)
                : string.IsNullOrWhiteSpace(x.WebSite));

        if (HasDuties.HasValue)
            Query.Where(x => HasDuties.Value
                ? !string.IsNullOrWhiteSpace(x.Duties)
                : string.IsNullOrWhiteSpace(x.Duties));

        if (IsPresentTime.HasValue)
            Query.Where(x => x.IsPresentTime.Equals(IsPresentTime));

        Initialize();
    }

    public int? ResumeId { get; set; }
    public IEnumerable<string>? SpecialityNames { get; set; }
    public IEnumerable<string>? Companies { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public bool? HasWebSite { get; set; }
    public bool? HasDuties { get; set; }
    public bool? IsPresentTime { get; set; }
}
