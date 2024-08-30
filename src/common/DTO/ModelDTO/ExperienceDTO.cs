using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace common.DTO.ModelDTO;

public class ExperienceDTO
{
    [Required]
    public string SpecialityName { get; set; } = null!;

    [Required]
    public string Company { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;

    public string? WebSite { get; set; }

    [Required]
    public DateTime StartOfWork { get; set; }

    public DateTime? EndOfWork { get; set; }

    [Required]
    public bool IsPresentTime { get; set; }

    public string? Duties { get; set; }
}
