using System.ComponentModel.DataAnnotations;

namespace common.DTO.ModelDTO.Chat;

public class ChatDTO
{
    [Required]
    public int CandidateId { get; set; }

    [Required]
    public int EmployerId { get; set; }
}
