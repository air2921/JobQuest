﻿using domain.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models
{
    [Table("Responses")]
    public class ResponseModel
    {
        [Key]
        public int ResponseId { get; set; }

        [ResponseStatus(ErrorMessage = "Неизвестный статус")]
        public int Status { get; set; }

        public DateTime ResponseOfDate { get; set; }

        [ForeignKey("ResumeId")]
        public int ResumeId { get; set; }

        [ForeignKey("VacancyId")]
        public int VacancyId { get; set; }

        [JsonIgnore]
        public ResumeModel? Resume { get; set; }

        [JsonIgnore]
        public VacancyModel? Vacancy { get; set; }
    }

    public enum StatusResponse
    {
        Expectation = 101,
        Invitation = 201,
        Refusal = 301
    }
}
