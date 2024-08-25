using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Auths")]
public class AuthModel
{
    [Key]
    public int TokenId { get; set; }

    [Column]
    public string Value { get; set; } = null!;

    [Column]
    public DateTime CreatedAt { get; set; }

    [Column]
    public DateTime Expires { get; set; }

    [NotMapped]
    public bool IsExpired
    {
        get => Expires < DateTime.UtcNow;
        private set { }
    }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UserModel? User { get; set; }
}
