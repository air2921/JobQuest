using System.ComponentModel.DataAnnotations;

namespace common.DTO;

public class JwtDTO
{
    [Required]
    public string Role { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    [Required]
    public TimeSpan Expires { get; set; }
}
