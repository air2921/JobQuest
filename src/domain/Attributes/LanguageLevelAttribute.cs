using domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class LanguageLevelAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int level))
            return false;

        var enumValues = Enum.GetValues(typeof(LanguageLevel));
        var numberValues = new List<int>();

        foreach (var enumValue in enumValues)
            numberValues.Add((int)enumValue);

        return numberValues.Contains(level);
    }
}
