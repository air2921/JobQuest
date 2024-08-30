using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models.Chat;

[Table("Chats")]
public class ChatModel
{
    [Key]
    [JsonPropertyName("chat_id")]
    public int ChatId { get; set; }

    [Column]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("CandidateId")]
    [JsonPropertyName("candidate_id")]
    public int CandidateId { get; set; }

    [ForeignKey("EmployerId")]
    [JsonPropertyName("employer_id")]
    public int EmployerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("candidate_user")]
    public ResumeModel CandidateUser { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("employer_user")]
    public CompanyModel EmployerUser { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("messages")]
    public ICollection<MessageModel> Messages { get; set; } = [];
}
