using domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using domain.Attributes;

namespace domain.Models;

[Table("LanguageResumes")]
public class LanguageResumeModel
{
    [Key]
    [JsonPropertyName("language_resume_id")]
    public int LanguageResumeId { get; set; }

    [Column]
    [LanguageLevel]
    [JsonPropertyName("level")]
    public int Level { get; set; }

    [ForeignKey("LanguageId")]
    [JsonPropertyName("language_id")]
    public int LanguageId { get; set; }

    [JsonIgnore]
    [JsonPropertyName("language")]
    public LanguageModel? Language { get; set; }

    [ForeignKey("ResumeId")]
    [JsonPropertyName("resume_id")]
    public int ResumeId { get; set; }

    [JsonIgnore]
    [JsonPropertyName("resume")]
    public ResumeModel? Resume { get; set; }
}
