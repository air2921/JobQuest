﻿using domain.Attributes;
using domain.Enums;
using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Resumes")]
public class ResumeModel : Contact
{
    [Key]
    public int ResumeId { get; set; }

    [Column]
    public string ResumeName { get; set; } = null!;

    [Column]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    public int MinSalary { get; set; } = 0;

    [Column]
    public int MaxSalary { get; set; } = 0;

    [Column]
    [JobStatus]
    public int Status { get; set; }

    [Column]
    [Employment]
    public int Employment { get; set; }

    [Column]
    [WorkSchedule]
    public int WorkSchedule { get; set; }

    [Column]
    public string Location { get; set; } = null!;

    [Column]
    public string Citizenship { get; set; } = null!;

    [Column]
    public string WorkPermit { get; set; } = null!;

    [Column]
    public string SpecialityName { get; set; } = null!;

    [Column]
    public string[]? Skills { get; set; }

    [Column]
    public string? About { get; set; }

    [Column]
    public string? ImageKey { get; set; }

    [Column]
    [Name(nullValidate: true)]
    public string FirstName { get; set; } = null!;

    [Column]
    [Name(nullValidate: false)]
    public string? Patronymic { get; set; }

    [Column]
    [Name(nullValidate: true)]
    public string LastName { get; set; } = null!;

    [Column]
    public bool IsMale { get; set; }

    [Column]
    public DateTime DateOfBirthday { get; set; }

    [NotMapped]
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirthday.Year;

            if (today < DateOfBirthday.AddYears(age))
                age--;

            return age;
        }
        private set { }
    }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UserModel? User { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ResponseModel>? Responses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<LanguageModel>? Languages { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<EducationModel>? Educations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ExperienceModel>? Experiences { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ChatModel>? CandidateChats { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<MessageModel>? SentMessagesAsCandidate { get; set; }
}
