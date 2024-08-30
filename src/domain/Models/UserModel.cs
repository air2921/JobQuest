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
    [JsonIgnore]
    public string Email { get; set; } = null!;

    [Column]
    [Hash]
    [JsonIgnore]
    public string PasswordHash { get; set; } = null!;

    [Column]
    [Role]
    [JsonIgnore]
    public string Role { get; set; } = null!;

    [Column]
    [JsonPropertyName("is_blocked")]
    public bool IsBlocked { get; set; } = false;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("company")]
    public CompanyModel? Company { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("reviews")]
    public ICollection<ReviewModel> Reviews { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("resumes")]
    public ICollection<ResumeModel> Resumes { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("languages")]
    public ICollection<LanguageModel> Languages { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("favorite_resumes")]
    public ICollection<FavoriteResumeModel> FavoriteResumes { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("favorite_vacancies")]
    public ICollection<FavoriteVacancyModel> FavoriteVacancies { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("auths")]
    public ICollection<AuthModel> Auths { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("recoveries")]
    public ICollection<RecoveryModel> Recoveries { get; set; } = [];
}
