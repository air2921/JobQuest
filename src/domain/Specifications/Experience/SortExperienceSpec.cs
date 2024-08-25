using Ardalis.Specification;
using domain.SpecDTO;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Experience;

public class SortExperienceSpec : SortCollectionSpec<ExperienceModel>
{
    public SortExperienceSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.ExperienceCountInMounts) 
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.ResumeId.HasValue)
            Query.Where(x => x.ResumeId.Equals(DTO.ResumeId));

        if (DTO.SpecialityNames is not null && DTO.SpecialityNames.Any())
            Query.Where(x => DTO.SpecialityNames.Contains(x.SpecialityName));

        if (DTO.Companies is not null && DTO.Companies.Any())
            Query.Where(x => DTO.Companies.Contains(x.Company));

        if (DTO.Locations is not null && DTO.Locations.Any())
            Query.Where(x => DTO.Locations.Contains(x.Location));

        if (DTO.HasWebSite.HasValue)
            Query.Where(x => DTO.HasWebSite.Value
                ? !string.IsNullOrWhiteSpace(x.WebSite)
                : string.IsNullOrWhiteSpace(x.WebSite));

        if (DTO.HasDuties.HasValue)
            Query.Where(x => DTO.HasDuties.Value
                ? !string.IsNullOrWhiteSpace(x.Duties)
                : string.IsNullOrWhiteSpace(x.Duties));

        if (DTO.IsPresentTime.HasValue)
            Query.Where(x => x.IsPresentTime.Equals(DTO.IsPresentTime));

        Initialize();
    }

    public SortExperienceDTO? DTO { get; set; }
}
