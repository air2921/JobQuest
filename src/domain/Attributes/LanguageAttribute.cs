using domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class LanguageAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (value is not string stringValue)
            return false;

        var enumValues = Enum.GetValues(typeof(Language)).Cast<Language>();
        var stringValues = enumValues.Select(e => e.ToString()).ToList();

        return stringValues.Contains(stringValue);
    }
}
