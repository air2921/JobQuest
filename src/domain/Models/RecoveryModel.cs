﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace domain.Models;

[Table("Recoveries")]
public class RecoveryModel
{
    [Key]
    public int TokenId { get; set; }

    public string Value { get; set; } = null!;

    public DateTime Expires { get; set; }

    [ForeignKey("UserId")]
    public int UserId { get; set; }

    [JsonIgnore]
    public UserModel? User { get; set; }
}
