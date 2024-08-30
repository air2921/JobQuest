using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace common.DTO.ModelDTO;

public class EducationDTO
{
    [Required]
    public int Level { get; set; }

    [Required]
    public string Institution { get; set; } = null!;

    [Required]
    public string Specialty { get; set; } = null!;

    [Required]
    public DateTime DateOfEnrollment { get; set; }

    public DateTime? DateOfGraduation { get; set; }

    [Required]
    public bool IsPresentTime { get; set; }
}
