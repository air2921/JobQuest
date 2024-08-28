using domain.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models;

public abstract class Contact
{
    [Column]
    [PhoneNumber(nullValidate: false)]
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column]
    [Email(nullValidate: false)]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    [JsonPropertyName("telegram")]
    public string? Telegram { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    [JsonPropertyName("github")]
    public string? Github { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    [JsonPropertyName("linkedin")]
    public string? LinkedIn { get; set; }

    [Column]
    [Attributes.Url(nullValidate: false)]
    [JsonPropertyName("website")]
    public string? WebSite { get; set; }
}
