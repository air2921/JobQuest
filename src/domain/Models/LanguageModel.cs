using domain.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Languages")]
public class LanguageModel
{
    [Key]
    [JsonPropertyName("language_id")]
    public int LanguageId { get; set; }

    [Column]
    [Language]
    [JsonPropertyName("language_name")]
    public string LanguageName { get; set; } = null!;

    [JsonIgnore]
    public ICollection<LanguageResumeModel>? LanguageResumes { get; set; }
}
