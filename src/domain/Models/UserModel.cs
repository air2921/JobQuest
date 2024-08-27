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
    public int UserId { get; set; }

    [Column]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    [Email(nullValidate: true)]
    public string Email { get; set; } = null!;

    [Column]
    [Hash]
    public string PasswordHash { get; set; } = null!;

    [Column]
    [Role]
    public string Role { get; set; } = null!;

    [Column]
    public bool IsBlocked { get; set; } = false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ReviewModel>? Reviews { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<CompanyModel>? Companies { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ResumeModel>? Resumes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<FavoriteModel>? Favorites { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<AuthModel>? Auths { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<RecoveryModel>? Recoveries { get; set; }
}
