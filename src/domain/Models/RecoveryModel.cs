using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Recoveries")]
public class RecoveryModel
{
    [Key]
    [JsonPropertyName("token_id")]
    public int TokenId { get; set; }

    [Column]
    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;

    [Column]
    [JsonPropertyName("is_used")]
    public bool IsUsed { get; set; } = false;

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column]
    [JsonPropertyName("expires")]
    public DateTime Expires { get; set; }

    [NotMapped]
    public bool IsExpired
    {
        get => Expires < DateTime.UtcNow;
        private set { }
    }

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("user")]
    public UserModel? User { get; set; }
}
