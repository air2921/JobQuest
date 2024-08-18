using domain.Attributes;
using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Resumes")]
public class ResumeModel : Personal
{
    [Key]
    public int ResumeId { get; set; }

    [Column]
    public string ResumeName { get; set; } = null!;

    [Column]
    public DateTime CreatedAt { get; set; }

    [Column]
    public int MinSalary { get; set; } = 0;

    [Column]
    public int MaxSalary { get; set; } = 0;

    [Column]
    [JobStatus(ErrorMessage = "Неизвестный статус")]
    public int Status { get; set; }

    [Column]
    [Employment(ErrorMessage = "Неизвестная занятость")]
    public int Employment { get; set; }

    [Column]
    [WorkSchedule(ErrorMessage = "Неизвестный график работы")]
    public int WorkSchedule { get; set; }

    [Column]
    public string Location { get; set; } = null!;

    [Column]
    public string Citizenship { get; set; } = null!;

    [Column]
    public string WorkPermit { get; set; } = null!;

    [Column]
    public string SpecializationName { get; set; } = null!;

    [Column]
    public HashSet<string>? Skills { get; set; }

    [Column]
    public Dictionary<string, string>? Languages { get; set; }

    [Column]
    public string? About { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UserModel? User { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ResponseModel>? Responses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<EducationModel>? Educations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ExperienceModel>? Experiences { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ChatModel>? CandidateChats { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<MessageModel>? SentMessagesAsCandidate { get; set; }
}

public class Contacts
{
    [Column]
    [PhoneNumber(nullValidate: false, ErrorMessage = "Неверный формат номера телефона")]
    public string? PhoneNumber { get; set; }

    [Column]
    [Email(nullValidate: false, ErrorMessage = "Неверный формат электронной почты")]
    public string? Email { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? Telegram { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? Github { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? LinkedIn { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? WebSite { get; set; }
}

public class Personal : Contacts
{
    [Column]
    public string? ImageKey { get; set; }

    [Column]
    [Name(nullValidate: true, ErrorMessage = "Неверный формат имени")]
    public string FirstName { get; set; } = null!;

    [Column]
    [Name(nullValidate: false, ErrorMessage = "Неверный формат имени")]
    public string? Patronymic { get; set; }

    [Column]
    [Name(nullValidate: true, ErrorMessage = "Неверный формат имени")]
    public string LastName { get; set; } = null!;

    [Column]
    public bool IsMale { get; set; }

    [Column]
    public DateTime DateOfBirthday { get; set; }
}