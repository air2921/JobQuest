using domain.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Models;

public abstract class Contact
{
    [Column]
    [PhoneNumber(nullValidate: false, ErrorMessage = "Неверный формат номера телефона")]
    public string? PhoneNumber { get; set; }

    [Column]
    [Email(nullValidate: false, ErrorMessage = "Неверный формат электронной почты")]
    public string? Email { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? Telegram { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? Github { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? LinkedIn { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false, ErrorMessage = "Неверный формат ссылки")]
    public string? WebSite { get; set; }
}
