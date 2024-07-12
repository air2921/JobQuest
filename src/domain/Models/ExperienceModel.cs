using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models
{
    [Table("Experience")]
    public class ExperienceModel
    {
        [Key]
        public int ExperienceId { get; set; }

        public string Company { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string? WebSite { get; set; }

        public DateTime StartOfWork { get; set; }

        public DateTime? EndOfWork { get; set; }

        public bool IsPresentTime { get; set; }

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

        public string? Duties { get; set; }

        [ForeignKey("ResumeId")]
        public int ResumeId { get; set; }

        [JsonIgnore]
        public ResumeModel? Resume { get; set; }
    }
}
