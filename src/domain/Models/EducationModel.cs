using domain.Attributes;
using domain.Localize;
using JsonLocalizer;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Educations")]
public class EducationModel
{
    private DateTime? _dateOfGraduation;
    private bool _isPresentTime;

    [Key]
    [JsonPropertyName("education_id")]
    public int EducationId { get; set; }

    [Column]
    [Education]
    [JsonPropertyName("level")]
    public int Level { get; set; }

    [Column]
    [JsonPropertyName("institution")]
    public string Institution { get; set; } = null!;

    [Column]
    [JsonPropertyName("specialty")]
    public string Specialty { get; set; } = null!;

    [Column]
    [JsonPropertyName("date_of_enrollment")]
    public DateTime DateOfEnrollment { get; set; }

    [Column]
    [JsonPropertyName("date_of_graduation")]
    public DateTime? DateOfGraduation
    {
        get => _dateOfGraduation;
        set
        {
            if (_isPresentTime && value is not null)
                throw new ValidationException(
                    Localizer.Translate(Validation.EDUCATION_DATE_OF_GRADUATION));

            _dateOfGraduation = value;
        }
    }

    [Column]
    [JsonPropertyName("is_present_time")]
    public bool IsPresentTime
    {
        get => _isPresentTime;
        set
        {
            if (value && _dateOfGraduation is not null)
                throw new ValidationException(
                    Localizer.Translate(Validation.EDUCATION_IS_PRESENT_TIME));

            _isPresentTime = value;
        }
    }

    [ForeignKey("ResumeId")]
    [JsonPropertyName("resume_id")]
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("resume")]
    public ResumeModel Resume { get; set; } = null!;
}
