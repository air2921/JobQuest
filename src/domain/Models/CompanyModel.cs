using domain.Attributes;
using domain.Models.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Companies")]
public class CompanyModel : Contact
{
    [Key]
    public int CompanyId { get; set; }

    [Column]
    public string CompanyName { get; set; } = null!;

    [Column]
    public string Location { get; set; } = null!;

    [Column]
    public DateTime RegisterDate { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public UserModel? User { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<VacancyModel>? Vacancies { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ReviewModel>? Reviews { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<ChatModel>? EmployerChats { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<MessageModel>? SentMessagesAsEmployer { get; set; }
}
