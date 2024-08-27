using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Auths")]
public class AuthModel
{
    [Key]
    [JsonPropertyName("token_id")]
    public int TokenId { get; set; }

    [Column]
    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column]
    [JsonPropertyName("expires")]
    public DateTime Expires { get; set; }

    [NotMapped]
    [JsonIgnore]
    public bool IsExpired
    {
        get => Expires < DateTime.UtcNow;
        private set { }
    }

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel? User { get; set; }
}
