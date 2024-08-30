using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace common.DTO.ModelDTO;

public class ResumeDTO
{
    [Required]
    public string ResumeName { get; set; } = null!;

    public int MinSalary { get; set; } = 0;

    public int MaxSalary { get; set; } = 0;

    [Required]
    public int Status { get; set; }

    [Required]
    public int Employment { get; set; }

    [Required]
    public int WorkSchedule { get; set; }

    [Required]
    public string Location { get; set; } = null!;

    [Required]
    public string Citizenship { get; set; } = null!;

    [Required]
    public string WorkPermit { get; set; } = null!;

    [Required]
    public string SpecialityName { get; set; } = null!;

    public string[]? Skills { get; set; }

    public string? About { get; set; }

    [JsonIgnore]
    public string? ImageKey { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public bool IsMale { get; set; }

    [Required]
    public DateTime DateOfBirthday { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Telegram { get; set; }

    public string? Github { get; set; }

    public string? LinkedIn { get; set; }

    public string? WebSite { get; set; }
}
