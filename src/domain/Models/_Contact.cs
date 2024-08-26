using domain.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.Models;

public abstract class Contact
{
    [Column]
    [PhoneNumber(nullValidate: false)]
    public string? PhoneNumber { get; set; }

    [Column]
    [Email(nullValidate: false)]
    public string? Email { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    public string? Telegram { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    public string? Github { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    public string? LinkedIn { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    public string? WebSite { get; set; }
}
