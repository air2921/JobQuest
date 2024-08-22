using System.Collections.Generic;

namespace domain.SpecDTO;

public class SortEducationDTO
{
    public int? ResumeId { get; set; }
    public IEnumerable<int>? Levels { get; set; }
    public IEnumerable<string>? Institutions { get; set; }
    public IEnumerable<string>? Specialties { get; set; }
    public bool? StillStyding { get; set; }
}
