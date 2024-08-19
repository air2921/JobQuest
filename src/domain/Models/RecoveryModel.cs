using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Recoveries")]
public class RecoveryModel
{
    [Key]
    public int TokenId { get; set; }

    [Column]
    public string Value { get; set; } = null!;

    [Column]
    public bool IsUsed { get; set; } = false;

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
