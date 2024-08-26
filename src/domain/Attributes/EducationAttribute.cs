using domain.Enums;
using domain.Localize;
using JsonLocalizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class EducationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = Localizer.Translate(Validation.EDUCATION);

        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int level))
            return false;

        var enumValues = Enum.GetValues(typeof(EducationLevel));
        var numberValues = new List<int>();

        foreach (var enumValue in enumValues)
            numberValues.Add((int)enumValue);

        return numberValues.Contains(level);
    }
}
