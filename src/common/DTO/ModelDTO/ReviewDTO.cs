using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace common.DTO.ModelDTO;

public class ReviewDTO
{
    [Required]
    public string JobTitle { get; set; } = null!;

    [Required]
    public int DurationOfWork { get; set; }

    [Required]
    public int HiringProcessGrade { get; set; }

    [Required]
    public int ManagementGrade { get; set; }

    [Required]
    public int SalaryGrade { get; set; }

    [Required]
    public int WorkConditionsGrade { get; set; }

    [Required]
    public int RestConditionsGrade { get; set; }

    [Required]
    public int WorkPlaceGrade { get; set; }

    [Required]
    public int TeamGrade { get; set; }

    [Required]
    public int GrowthOpportunitiesGrade { get; set; }

    [Required]
    public bool IsRecomended { get; set; }

    [Required]
    public string? Description { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }
}
