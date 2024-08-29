using System.Collections.Generic;

namespace domain.SpecDTO;

public class SortVacancyDTO : PaginationDTO
{
    public int? CompanyId { get; set; }
    public bool? IsOpened { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public IEnumerable<int>? EducationLevels { get; set; }
    public IEnumerable<int>? Employments { get; set; }
    public IEnumerable<int>? WorkSchedules { get; set; }
    public string? Name { get; set; }
}
