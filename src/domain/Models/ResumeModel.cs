using domain.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models
{
    [Table("Resumes")]
    public class ResumeModel : Personal
    {
        [Key]
        public int ResumeId { get; set; }

        public string ResumeName { get; set; }

        public int MinSalary { get; set; } = 0;

        public int MaxSalary { get; set; } = 0;

        [JobStatus(ErrorMessage = "Неизвестный статус")]
        public int Status { get; set; }

        [Employment(ErrorMessage = "Неизвестная занятость")]
        public int Employment { get; set; }

        [WorkSchedule(ErrorMessage = "Неизвестный график работы")]
        public int WorkSchedule { get; set; }

        public string Location { get; set; }

        public string Citizenship { get; set; }

        public string WorkPermit { get; set; }

        public string SpecializationName { get; set; }

        public HashSet<string>? Skills { get; set; }

        public Dictionary<string, string> Languages { get; set; }

        public string? About { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }

        [JsonIgnore]
        public ICollection<ResponseModel>? Responses { get; set; }

        [JsonIgnore]
        public ICollection<EducationModel>? Educations { get; set; }

        [JsonIgnore]
        public ICollection<ExperienceModel>? Experiences { get; set; }
    }

    public class Contacts
    {
        [PhoneNumber(nullValidate: false, ErrorMessage = "Неверный формат номера телефона")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        public string? Email { get; set; }

        [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
        public string? Telegram { get; set; }

        [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
        public string? Github { get; set; }

        [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
        public string? LinkedIn { get; set; }

        [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
        public string? WebSite { get; set; }
    }

    public class Personal : Contacts
    {
        public string? ImageKey { get; set; }

        public string FirstName { get; set; }

        public string? Patronymic { get; set; }

        public string LastName { get; set; }

        public bool IsMale { get; set; }

        public DateTime DateOfBirthday { get; set; }
    }

    public enum JobStatus
    {
        Actively,
        Сonsidering,
        Think,
        Accepted,
        NoSearching
    }
}
