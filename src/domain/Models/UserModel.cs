using domain.Attributes;
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

    [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
    public string Email { get; set; } = null!;

    [Hash(ErrorMessage = "Ошибка при создании пароля, возможно пароль имеет неподдерживаемые символы")]
    public string PasswordHash { get; set; } = null!;

    [Role(ErrorMessage = "Такой роли не существует")]
    public string Role { get; set; } = null!;

    public bool IsBlocked { get; set; }

    [JsonIgnore]
    public ICollection<CompanyModel>? Companies { get; set; }

    [JsonIgnore]
    public ICollection<ResumeModel>? Resumes { get; set; }

    [JsonIgnore]
    public ICollection<FavoriteModel>? Favorites { get; set; }

    [JsonIgnore]
    public ICollection<AuthModel>? Auths { get; set; }

    [JsonIgnore]
    public ICollection<RecoveryModel>? Recoveries { get; set; }
}

public enum Role
{
    Employer,
    Candidate,
    Admin
}
