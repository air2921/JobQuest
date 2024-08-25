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
    public int ChatId { get; set; }

    [Column]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("CandidateId")]
    public int CandidateId { get; set; }

    [ForeignKey("EmployerId")]
    public int EmployerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ResumeModel? CandidateUser { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public CompanyModel? EmployerUser { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ICollection<MessageModel>? Messages { get; set; }
}
