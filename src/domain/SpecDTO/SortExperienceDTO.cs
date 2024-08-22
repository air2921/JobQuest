using System.Collections.Generic;

namespace domain.SpecDTO;

public class SortExperienceDTO
{
    public int? ResumeId { get; set; }
    public IEnumerable<string>? SpecialityNames { get; set; }
    public IEnumerable<string>? Companies { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public bool? HasWebSite { get; set; }
    public bool? HasDuties { get; set; }
    public bool? IsPresentTime { get; set; }
}
