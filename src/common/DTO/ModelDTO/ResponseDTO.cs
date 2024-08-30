using System.ComponentModel.DataAnnotations;

namespace common.DTO.ModelDTO;

public class ResponseDTO
{
    public int Status { get; set; } = 101;

    [Required]
    public DateTime ResponseOfDate { get; set; }

    [Required]
    public int? Reason { get; set; }

    public string? ReasonDescription { get; set; }

    [Required]
    public int ResumeId { get; set; }

    [Required]
    public int VacancyId { get; set; }
}
