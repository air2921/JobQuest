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
    [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
    public string Email { get; set; } = null!;

    [Column]
    [Hash(ErrorMessage = "Ошибка при создании пароля, возможно пароль имеет неподдерживаемые символы")]
    public string PasswordHash { get; set; } = null!;

    [Column]
    [Role(ErrorMessage = "Такой роли не существует")]
    public string Role { get; set; } = null!;

    [Column]
    public bool IsBlocked { get; set; }

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

public enum Role
{
    Employer,
    Candidate,
    Admin
}
