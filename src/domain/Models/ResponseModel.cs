using domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Responses")]
public class ResponseModel
{
    [Key]
    public int ResponseId { get; set; }

    [Column]
    [ResponseStatus(ErrorMessage = "Неизвестный статус")]
    public int Status { get; set; }

    [Column]
    public DateTime ResponseOfDate { get; set; }

    [ForeignKey("ResumeId")]
    public int ResumeId { get; set; }

    [ForeignKey("VacancyId")]
    public int VacancyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? Resume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public VacancyModel? Vacancy { get; set; }
}

public enum StatusResponse
{
    Expectation = 101,
    Invitation = 201,
    Refusal = 301
}
