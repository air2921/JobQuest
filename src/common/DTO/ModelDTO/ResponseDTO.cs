namespace common.DTO.ModelDTO;

public class ResponseDTO
{
    public int Status { get; set; } = 101;

    public DateTime ResponseOfDate { get; set; }

    public int? Reason { get; set; }

    public string? ReasonDescription { get; set; }

    public int ResumeId { get; set; }

    public int VacancyId { get; set; }
}
