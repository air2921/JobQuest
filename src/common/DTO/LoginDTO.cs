using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.DTO;

public class LoginDTO
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
}
