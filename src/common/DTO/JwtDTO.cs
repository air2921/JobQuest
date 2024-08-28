using System.ComponentModel.DataAnnotations;

namespace common.DTO;

public class JwtDTO
{
    [Required]
    public string Role { get; set; } = null!;

    public int? CompanyId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public TimeSpan Expires { get; set; }
}
