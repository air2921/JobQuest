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
    public int VacancyId { get; set; }

    [Column]
    public bool IsOpened { get; set; } = true;

    [Column]
    public string VacancyName { get; set; } = null!;

    [Column]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    public string Location { get; set; } = null!;

    [Column]
    public int MinSalary { get; set; } = 0;

    [Column]
    public int MaxSalary { get; set; } = 0;

    [Column]
    public int Experience { get; set; } = 0;

    [Column]
    [Education(ErrorMessage = "Неизвестное образование")]
    public int EducationLevel { get; set; }

    [Column]
    [Employment(ErrorMessage = "Неизвестная занятость")]
    public int Employment { get; set; }

    [Column]
    [WorkSchedule(ErrorMessage = "Неизвестный график работы")]
    public int WorkSchedule { get; set; }

    [Column]
    public string About { get; set; } = null!;

    [ForeignKey("CompanyId")]
    public int CompanyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompanyModel? Company { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ResponseModel>? Responses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<FavoriteModel>? Favorites { get; set; }
}
