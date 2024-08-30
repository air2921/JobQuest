using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace domain.Models;

public class FavoriteResumeModel
{
    [Key]
    [JsonPropertyName("favorite_resume_id")]
    public int FavoriteId { get; set; }

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [ForeignKey("ResumeId")]
    [JsonPropertyName("resume_id")]
    public int ResumeId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel User { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("resume")]
    public ResumeModel Resume { get; set; } = null!;
}
