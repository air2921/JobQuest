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
    public string VacancyName { get; set; } = null!;

    [Column]
    public DateTime CreatedAt { get; set; }

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
    public string? About { get; set; }

    [ForeignKey("CompanyId")]
    public int CompanyId { get; set; }

    [JsonIgnore]
    public CompanyModel? Company { get; set; }

    [JsonIgnore]
    public ICollection<ResponseModel>? Responses { get; set; }

    [JsonIgnore]
    public ICollection<FavoriteModel>? Favorites { get; set; }
}

public enum Employment
{
    Full = 101,
    Partial = 102,
    Internship = 103,
    Project = 104
}

public enum WorkSchedule
{
    FullDay = 101,
    Remote = 102,
    Flexible = 103
}

public enum EducationLevel
{
    Secondary = 101,
    Vocational = 102,
    IncompleteHigher = 201,
    Higher = 202,
    Bachelor = 203,
    Master = 204,
    PhD = 301,
    DoF = 302
}
