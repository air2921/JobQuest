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

        public string WebSite { get; set; } = null!;

        public DateTime StartOfWork { get; set; }

        public DateTime? EndOfWork { get; set; }

        public bool IsPresentTime { get; set; }

        public string? Duties { get; set; }

        [ForeignKey("ResumeId")]
        public int ResumeId { get; set; }

        [JsonIgnore]
        public ResumeModel? Resume { get; set; }
    }
}
