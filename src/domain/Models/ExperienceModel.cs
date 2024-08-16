using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Experience")]
public class ExperienceModel
{
    [Key]
    public int ExperienceId { get; set; }

    [Column]
    public string Company { get; set; } = null!;

    [Column]
    public string Location { get; set; } = null!;

    [Column]
    public string? WebSite { get; set; }

    [Column]
    public DateTime StartOfWork { get; set; }

    [Column]
    public DateTime? EndOfWork { get; set; }

    [Column]
    public bool IsPresentTime { get; set; }

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

    [JsonIgnore]
    public ResumeModel? Resume { get; set; }
}
