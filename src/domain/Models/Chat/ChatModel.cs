using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models.Chat
{
    [Table("Chats")]
    public class ChatModel
    {
        [Key]
        public int ChatId { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("CandidateId")]
        public int CandidateId { get; set; }

        [ForeignKey("EmployerId")]
        public int EmployerId { get; set; }

        [JsonIgnore]
        public UserModel? CandidateUser { get; set; }

        [JsonIgnore]
        public CompanyModel? EmployerUser { get; set; }

        [JsonIgnore]
        public ICollection<MessageModel>? Messages { get; set; }
    }
}
