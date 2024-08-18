using domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Educations")]
public class EducationModel
{
    [Key]
    public int EducationId { get; set; }

    [Column]
    [Education(ErrorMessage = "Неизвестное образование")]
    public int Level { get; set; }

    [Column]
    public string Institution { get; set; } = null!;

    [Column]
    public DateTime DateOfEnrollment { get; set; }

    [Column]
    public DateTime DateOfGraduation { get; set; }

    [ForeignKey("ResumeId")]
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? Resume { get; set; }
}
