using System.ComponentModel.DataAnnotations;

namespace common.DTO;

public class EmailDTO
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Subject { get; set; } = null!;

    [Required]
    public string Body { get; set; } = null!;
}
