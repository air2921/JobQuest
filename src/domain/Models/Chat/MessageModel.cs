using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models.Chat;

[Table("Messages")]
public class MessageModel
{
    [Key]
    public int MessageId { get; set; }

    [Column]
    public string Message { get; set; } = null!;

    [Column]
    public bool IsRead { get; set; } = false;

    [Column]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ChatId")]
    public int ChatId { get; set; }

    [ForeignKey("EmployerId")]
    public int EmployerId { get; set; }

    [ForeignKey("CandidateId")]
    public int CandidateId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ChatModel? Chat { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompanyModel? Employer { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? Candidate { get; set; }
}
