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
    [JsonPropertyName("company_id")]
    public int CompanyId { get; set; }

    [Column]
    [JsonPropertyName("company_name")]
    public string CompanyName { get; set; } = null!;

    [Column]
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [Column]
    [JsonPropertyName("register_date")]
    public DateTime RegisterDate { get; set; }

    [ForeignKey("UserId")]
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("user")]
    public UserModel User { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("vacancies")]
    public ICollection<VacancyModel> Vacancies { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("reviews")]
    public ICollection<ReviewModel> Reviews { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("employer_chats")]
    public ICollection<ChatModel> EmployerChats { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("sent_messages_as_employer")]
    public ICollection<MessageModel> SentMessagesAsEmployer { get; set; } = [];
}
