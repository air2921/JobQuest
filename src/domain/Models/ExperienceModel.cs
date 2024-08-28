using domain.Localize;
using JsonLocalizer;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Experience")]
public class ExperienceModel
{
    private DateTime? _endOfWork;
    private bool _isPresentTime;

    [Key]
    [JsonPropertyName("experience_id")]
    public int ExperienceId { get; set; }

    [Column]
    [JsonPropertyName("speciality_name")]
    public string SpecialityName { get; set; } = null!;

    [Column]
    [JsonPropertyName("company")]
    public string Company { get; set; } = null!;

    [Column]
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [Column]
    [Attributes.Url(nullValidate: false)]
    [JsonPropertyName("website")]
    public string? WebSite { get; set; }

    [Column]
    [JsonPropertyName("start_of_work")]
    public DateTime StartOfWork { get; set; }

    [Column]
    [JsonPropertyName("end_of_work")]
    public DateTime? EndOfWork
    {
        get => _endOfWork;
        set
        {
            if (_isPresentTime && value is not null)
                throw new ValidationException(
                    Localizer.Translate(Validation.EXPERIENCE_END_OF_WORK));

            _endOfWork = value;
        }
    }

    [Column]
    [JsonPropertyName("is_present_time")]
    public bool IsPresentTime
    {
        get => _isPresentTime;
        set
        {
            if (value && _endOfWork is not null)
                throw new ValidationException(
                    Localizer.Translate(Validation.EXPERIENCE_IS_PRESENT_TIME));

            _isPresentTime = value;
        }
    }

    [NotMapped]
    [JsonPropertyName("experience_count_in_months")]
    public int ExperienceCountInMounts
    {
        get
        {
            if (EndOfWork is not null && !IsPresentTime)
                return (EndOfWork.Value - StartOfWork).Days / 30;
            else if (EndOfWork is null && IsPresentTime)
                return (DateTime.UtcNow - StartOfWork).Days / 30;
            else
                return 0;
        }
        private set { }
    }

    [Column]
    [JsonPropertyName("duties")]
    public string? Duties { get; set; }

    [ForeignKey("ResumeId")]
    [JsonPropertyName("resume_id")]
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("resume")]
    public ResumeModel? Resume { get; set; }
}
