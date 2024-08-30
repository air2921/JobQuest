using domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Reviews")]
public class ReviewModel
{
    [Key]
    [JsonPropertyName("review_id")]
    public int ReviewId { get; set; }

    [Column]
    [JsonPropertyName("job_title")]
    public string JobTitle { get; set; } = null!;

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    [Grade]
    [JsonPropertyName("duration_of_work")]
    public int DurationOfWork { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("hiring_process_grade")]
    public int HiringProcessGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("management_grade")]
    public int ManagementGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("salary_grade")]
    public int SalaryGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("work_conditions_grade")]
    public int WorkConditionsGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("rest_conditions_grade")]
    public int RestConditionsGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("work_place_grade")]
    public int WorkPlaceGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("team_grade")]
    public int TeamGrade { get; set; }

    [Column]
    [Grade]
    [JsonPropertyName("growth_opportunities_grade")]
    public int GrowthOpportunitiesGrade { get; set; }

    [NotMapped]
    [JsonPropertyName("overall_grade")]
    public double OverallGrade
    {
        get
        {
            int[] grades = [HiringProcessGrade, ManagementGrade, SalaryGrade, WorkConditionsGrade,
                RestConditionsGrade, WorkPlaceGrade, TeamGrade, GrowthOpportunitiesGrade];

            int sum = grades.Sum();
            return sum != 0 ? sum / grades.Length : 0;
        }
    }

    [Column]
    [JsonPropertyName("is_recommended")]
    public bool IsRecomended { get; set; }

    [Column]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [ForeignKey("CompanyId")]
    [JsonPropertyName("company_id")]
    public int CompanyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel User { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("company")]
    public CompanyModel Company { get; set; } = null!;
}
