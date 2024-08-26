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
    public int ExperienceId { get; set; }

    [Column]
    public string SpecialityName { get; set; } = null!;

    [Column]
    public string Company { get; set; } = null!;

    [Column]
    public string Location { get; set; } = null!;

    [Column]
    [Attributes.Url(nullValidate: false)]
    public string? WebSite { get; set; }

    [Column]
    public DateTime StartOfWork { get; set; }

    [Column]
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
    public string? Duties { get; set; }

    [ForeignKey("ResumeId")]
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? Resume { get; set; }
}
