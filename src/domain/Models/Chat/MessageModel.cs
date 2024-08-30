using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models.Chat;

[Table("Messages")]
public class MessageModel
{
    [Key]
    [JsonPropertyName("message_id")]
    public int MessageId { get; set; }

    [Column]
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [Column]
    [JsonPropertyName("is_read")]
    public bool IsRead { get; set; } = false;

    [Column]
    [JsonPropertyName("sent_at")]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ChatId")]
    [JsonPropertyName("chat_id")]
    public int ChatId { get; set; }

    [ForeignKey("EmployerId")]
    [JsonPropertyName("employer_id")]
    public int EmployerId { get; set; }

    [ForeignKey("CandidateId")]
    [JsonPropertyName("candidate_id")]
    public int CandidateId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("chat")]
    public ChatModel Chat { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("employer")]
    public CompanyModel Employer { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("candidate")]
    public ResumeModel Candidate { get; set; } = null!;
}
