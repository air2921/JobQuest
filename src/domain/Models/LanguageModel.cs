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
    [JsonPropertyName("language")]
    public string Language { get; set; } = null!;

    [Column]
    [LanguageLevel]
    [JsonPropertyName("language_level")]
    public int LanguageLevel { get; set; }

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel? User { get; set; }
}
