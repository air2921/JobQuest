using domain.Enums;
using System.Collections.Generic;

namespace domain.SpecDTO;

public class SortResumeDTO : PaginationDTO
{
    public int? UserId { get; set; }

    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }

    public IEnumerable<int>? Statuses { get; set; }
    public IEnumerable<int>? Employments { get; set; }
    public IEnumerable<int>? WorkSchedules { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public IEnumerable<string>? Citizenships { get; set; }
    public IEnumerable<string>? WorkPermits { get; set; }

    public IEnumerable<string>? SpecialityNames { get; set; }
    public IEnumerable<string>? Skills { get; set; }
    public Dictionary<Language, LanguageLevel>? Languages { get; set; }

    public int? MinExp { get; set; }
    public int? MaxExp { get; set; }
    public bool? StillWorks { get; set; }
    public bool? HasDuties { get; set; }

    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }

    public bool? HasAbout { get; set; }
    public bool? HasPhoto { get; set; }
    public bool? IsMale { get; set; }

    public bool? HasPhoneNumber { get; set; }
    public bool? HasEmail { get; set; }
    public bool? HasTelegram { get; set; }
    public bool? HasGithub { get; set; }
    public bool? HasLinkedIn { get; set; }
    public bool? HasWebSite { get; set; }
}
