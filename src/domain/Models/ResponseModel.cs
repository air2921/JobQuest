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
    [JsonPropertyName("response_id")]
    public int ResponseId { get; set; }

    [Column]
    [ResponseStatus]
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [Column]
    [JsonPropertyName("response_of_date")]
    public DateTime ResponseOfDate { get; set; }

    [Column]
    [Reason(nullValidate: false)]
    [JsonPropertyName("reason")]
    public int? Reason { get; set; }

    [Column]
    [JsonPropertyName("reason_description")]
    public string? ReasonDescription { get; set; }

    [ForeignKey("ResumeId")]
    [JsonPropertyName("resume_id")]
    public int ResumeId { get; set; }

    [ForeignKey("VacancyId")]
    [JsonPropertyName("vacancy_id")]
    public int VacancyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("resume")]
    public ResumeModel? Resume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("vacancy")]
    public VacancyModel? Vacancy { get; set; }
}
