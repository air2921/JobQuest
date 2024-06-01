﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models.Chat
{
    [Table("Messages")]
    public class MessageModel
    {
        [Key]
        public int MessageId { get; set; }

        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }

        public DateTime SentAt { get; set; }

        [ForeignKey("ChatId")]
        public int ChatId { get; set; }

        [ForeignKey("EmployerId")]
        public string EmployerId { get; set; } = null!;

        [ForeignKey("CandidateId")]
        public string CandidateId { get; set; } = null!;

        [JsonIgnore]
        public ChatModel? Chat { get; set; }

        [JsonIgnore]
        public CompanyModel? Employer { get; set; }

        [JsonIgnore]
        public UserModel? Candidate { get; set; }
    }
}
