using domain.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Users")]
public class UserModel
{
    [Key]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    [Email(nullValidate: true)]
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [Column]
    [Hash]
    [JsonPropertyName("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column]
    [Role]
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;

    [Column]
    [JsonPropertyName("is_blocked")]
    public bool IsBlocked { get; set; } = false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("reviews")]
    public ICollection<ReviewModel>? Reviews { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("company")]
    public CompanyModel? Company { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("resumes")]
    public ICollection<ResumeModel>? Resumes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("favorites")]
    public ICollection<FavoriteModel>? Favorites { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("auths")]
    public ICollection<AuthModel>? Auths { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("recoveries")]
    public ICollection<RecoveryModel>? Recoveries { get; set; }
}
