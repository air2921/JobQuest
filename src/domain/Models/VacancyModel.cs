using domain.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Vacancies")]
public class VacancyModel
{
    [Key]
    [JsonPropertyName("vacancy_id")]
    public int VacancyId { get; set; }

    [Column]
    [JsonPropertyName("is_opened")]
    public bool IsOpened { get; set; } = true;

    [Column]
    [JsonPropertyName("vacancy_name")]
    public string VacancyName { get; set; } = null!;

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [Column]
    [JsonPropertyName("min_salary")]
    public int MinSalary { get; set; } = 0;

    [Column]
    [JsonPropertyName("max_salary")]
    public int MaxSalary { get; set; } = 0;

    [Column]
    [JsonPropertyName("experience")]
    public int Experience { get; set; } = 0;

    [Column]
    [Education]
    [JsonPropertyName("education_level")]
    public int EducationLevel { get; set; }

    [Column]
    [Employment]
    [JsonPropertyName("employment")]
    public int Employment { get; set; }

    [Column]
    [WorkSchedule]
    [JsonPropertyName("work_schedule")]
    public int WorkSchedule { get; set; }

    [Column]
    [JsonPropertyName("about")]
    public string About { get; set; } = null!;

    [ForeignKey("CompanyId")]
    [JsonPropertyName("company_id")]
    public int CompanyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("company")]
    public CompanyModel? Company { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("responses")]
    public ICollection<ResponseModel>? Responses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("favorites")]
    public ICollection<FavoriteModel>? Favorites { get; set; }
}
