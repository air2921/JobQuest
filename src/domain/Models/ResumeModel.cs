using domain.Attributes;
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
    [JsonPropertyName("resume_id")]
    public int ResumeId { get; set; }

    [Column]
    [JsonPropertyName("resume_name")]
    public string ResumeName { get; set; } = null!;

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column]
    [JsonPropertyName("min_salary")]
    public int MinSalary { get; set; } = 0;

    [Column]
    [JsonPropertyName("max_salary")]
    public int MaxSalary { get; set; } = 0;

    [Column]
    [JobStatus]
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [Column]
    [Employment]
    [JsonPropertyName("employment")]
    public int Employment { get; set; }

    [Column]
    [WorkSchedule]
    [JsonPropertyName("work_schedule")]
    public int WorkSchedule { get; set; }

    [Column]
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [Column]
    [JsonPropertyName("citizenship")]
    public string Citizenship { get; set; } = null!;

    [Column]
    [JsonPropertyName("work_permit")]
    public string WorkPermit { get; set; } = null!;

    [Column]
    [JsonPropertyName("speciality_name")]
    public string SpecialityName { get; set; } = null!;

    [Column]
    [JsonPropertyName("skills")]
    public string[]? Skills { get; set; }

    [Column]
    [JsonPropertyName("about")]
    public string? About { get; set; }

    [Column]
    [JsonPropertyName("image_key")]
    public string? ImageKey { get; set; }

    [Column]
    [Name(nullValidate: true)]
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = null!;

    [Column]
    [Name(nullValidate: false)]
    [JsonPropertyName("patronymic")]
    public string? Patronymic { get; set; }

    [Column]
    [Name(nullValidate: true)]
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = null!;

    [Column]
    [JsonPropertyName("is_male")]
    public bool IsMale { get; set; }

    [Column]
    [JsonPropertyName("date_of_birthday")]
    public DateTime DateOfBirthday { get; set; }

    [NotMapped]
    [JsonPropertyName("age")]
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
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel? User { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("responses")]
    public ICollection<ResponseModel>? Responses { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("language_resumes")]
    public ICollection<LanguageResumeModel>? LanguageResumes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("educations")]
    public ICollection<EducationModel>? Educations { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("experiences")]
    public ICollection<ExperienceModel>? Experiences { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("candidate_chats")]
    public ICollection<ChatModel>? CandidateChats { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("sent_messages_as_candidate")]
    public ICollection<MessageModel>? SentMessagesAsCandidate { get; set; }
}
