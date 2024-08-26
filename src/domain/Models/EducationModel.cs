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
    public int EducationId { get; set; }

    [Column]
    [Education]
    public int Level { get; set; }

    [Column]
    public string Institution { get; set; } = null!;

    [Column]
    public string Specialty { get; set; } = null!;

    [Column]
    public DateTime DateOfEnrollment { get; set; }

    [Column]
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
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? Resume { get; set; }
}
