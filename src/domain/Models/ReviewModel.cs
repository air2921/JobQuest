using domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace domain.Models
{
    [Table("Reviews")]
    public class ReviewModel
    {
        [Key]
        public int ReviewId { get; set; }

        public string JobTitle { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int DurationOfWork { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int HiringProcessGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int ManagementGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int SalaryGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int WorkConditionsGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int RestConditionsGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int WorkPlaceGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int TeamGrade { get; set; }

        [Grade(ErrorMessage = "Оценка должна быть от 1 до 5")]
        public int GrowthOpportunitiesGrade { get; set; }

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

        public bool IsRecomended { get; set; }

        public string? Description { get; set; }

        [ForeignKey("CompanyId")]
        public int CompanyId { get; set; }

        [JsonIgnore]
        public CompanyModel? Company { get; set; }
    }
}
