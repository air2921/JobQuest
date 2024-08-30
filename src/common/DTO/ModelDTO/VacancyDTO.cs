using System.ComponentModel.DataAnnotations;

namespace common.DTO.ModelDTO;

public class VacancyDTO
{
    [Required]
    public string VacancyName { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;

    public int MinSalary { get; set; } = 0;

    public int MaxSalary { get; set; } = 0;

    public int Experience { get; set; } = 0;

    [Required]
    public int EducationLevel { get; set; }

    [Required]
    public int Employment { get; set; }

    [Required]
    public int WorkSchedule { get; set; }

    [Required]
    public string About { get; set; } = null!;
}
