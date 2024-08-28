﻿using System.ComponentModel.DataAnnotations;

namespace common.DTO;

public class RegisterDTO
{
    private string _email = null!;

    [Required]
    public string Email
    {
        get => _email;
        set => _email = value.ToLowerInvariant();
    }

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public bool AsEmployer { get; set; } = false;
}
