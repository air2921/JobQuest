using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace common.DTO.ModelDTO;

public class CompanyDTO
{
    [Required]
    public string CompanyName { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;

    [Required]
    public DateTime RegisterDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Telegram { get; set; }

    public string? Github { get; set; }

    public string? LinkedIn { get; set; }

    public string? WebSite { get; set; }
}
