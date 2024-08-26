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
    public int ReviewId { get; set; }

    [Column]
    public string JobTitle { get; set; } = null!;

    [Column]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    [Grade]
    public int DurationOfWork { get; set; }

    [Column]
    [Grade]
    public int HiringProcessGrade { get; set; }

    [Column]
    [Grade]
    public int ManagementGrade { get; set; }

    [Column]
    [Grade]
    public int SalaryGrade { get; set; }

    [Column]
    [Grade]
    public int WorkConditionsGrade { get; set; }

    [Column]
    [Grade]
    public int RestConditionsGrade { get; set; }

    [Column]
    [Grade]
    public int WorkPlaceGrade { get; set; }

    [Column]
    [Grade]
    public int TeamGrade { get; set; }

    [Column]
    [Grade]
    public int GrowthOpportunitiesGrade { get; set; }

    [NotMapped]
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
    public bool IsRecomended { get; set; }

    [Column]
    public string? Description { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UserModel? User { get; set; }

    [ForeignKey("CompanyId")]
    public int CompanyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompanyModel? Company { get; set; }
}
