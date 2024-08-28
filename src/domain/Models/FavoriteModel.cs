using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Favorites")]
public class FavoriteModel
{
    [Key]
    [JsonPropertyName("favorite_id")]
    public int FavoriteId { get; set; }

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [ForeignKey("VacancyId")]
    [JsonPropertyName("vacancy_id")]
    public int VacancyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel? User { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("vacancy")]
    public VacancyModel? Vacancy { get; set; }
}
