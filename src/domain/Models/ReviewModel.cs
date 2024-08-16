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
    public DateTime CreatedAt { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int DurationOfWork { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int HiringProcessGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int ManagementGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int SalaryGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int WorkConditionsGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int RestConditionsGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int WorkPlaceGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
    public int TeamGrade { get; set; }

    [Column]
    [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
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

    [ForeignKey("CompanyId")]
    public int CompanyId { get; set; }

    [JsonIgnore]
    public CompanyModel? Company { get; set; }
}
