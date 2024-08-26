using System;
using JsonLocalizer;
using domain.Localize;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UrlAttribute(bool nullValidate) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = Localizer.Translate(Validation.URL);

        if (value is null)
            return !nullValidate;

        if (string.IsNullOrWhiteSpace(value.ToString()))
            return false;

        var urlPattern = @"^(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
        return Regex.IsMatch(value.ToString()!, urlPattern);
    }
}
