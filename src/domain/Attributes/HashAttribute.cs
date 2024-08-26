using System;
using JsonLocalizer;
using domain.Localize;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HashAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = Localizer.Translate(Validation.HASH);

        if (value is null || value is not string)
            return false;

        string passwordHash = (string)value;
        string bcryptPattern = @"^\$2[ayb]\$.{56}$";

        return Regex.IsMatch(passwordHash, bcryptPattern);
    }
}
