using domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models
{
    [Table("Educations")]
    public class EducationModel
    {
        [Key]
        public int EducationId { get; set; }

        [Education(ErrorMessage = "Неизвестное образование")]
        public int Level { get; set; }

        public string Institution { get; set; }

        public DateTime DateOfEnrollment { get; set; }

        public DateTime DateOfGraduation { get; set; }

        [ForeignKey("ResumeId")]
        public int ResumeId { get; set; }

        [JsonIgnore]
        public ResumeModel? Resume { get; set; }
    }
}
