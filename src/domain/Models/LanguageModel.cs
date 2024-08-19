using domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Languages")]
public class LanguageModel
{
    [Key]
    public int LanguageKey { get; set; }

    [Column]
    [Language(ErrorMessage = "Неизвестный язык")]
    public string LanguageName { get; set; } = null!;

    [Column]
    [LanguageLevel(ErrorMessage = "Неизвестный уровень языка")]
    public int Level { get; set; }

    [ForeignKey("ResumeId")]
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? Resume { get; set; }
}
