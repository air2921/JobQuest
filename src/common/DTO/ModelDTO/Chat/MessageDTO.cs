using System.ComponentModel.DataAnnotations;

namespace common.DTO.ModelDTO.Chat;

public class MessageDTO
{
    [Required]
    public string Message { get; set; } = null!;

    [Required]
    public int ChatId { get; set; }

    [Required]
    public int EmployerId { get; set; }

    [Required]
    public int CandidateId { get; set; }
}
