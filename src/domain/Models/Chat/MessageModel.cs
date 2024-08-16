﻿using System;
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
    public bool IsRead { get; set; }

    [Column]
    public DateTime SentAt { get; set; }

    [ForeignKey("ChatId")]
    public int ChatId { get; set; }

    [ForeignKey("EmployerId")]
    public int EmployerId { get; set; }

    [ForeignKey("CandidateId")]
    public int CandidateId { get; set; }

    [JsonIgnore]
    public ChatModel? Chat { get; set; }

    [JsonIgnore]
    public CompanyModel? Employer { get; set; }

    [JsonIgnore]
    public ResumeModel? Candidate { get; set; }
}
