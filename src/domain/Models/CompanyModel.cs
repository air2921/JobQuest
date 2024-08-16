using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Companies")]
public class CompanyModel
{
    [Key]
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Email { get; set; }

    public string Location { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime RegisterDate { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore]
    public UserModel? User { get; set; }

    [JsonIgnore]
    public ICollection<VacancyModel>? Vacancies { get; set; }

    [JsonIgnore]
    public ICollection<ReviewModel>? Reviews { get; set; }

    [JsonIgnore]
    public ICollection<ChatModel>? EmployerChats { get; set; }

    [JsonIgnore]
    public ICollection<MessageModel>? SentMessagesAsEmployer { get; set; }
}
