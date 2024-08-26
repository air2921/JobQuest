using domain.Enums;
using JsonLocalizer;
using domain.Localize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ResponseStatusAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = Localizer.Translate(Validation.RESPONSE);

        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int status))
            return false;

        var enumValues = Enum.GetValues(typeof(StatusResponse));
        var numberValues = new List<int>();

        foreach (var enumValue in enumValues)
            numberValues.Add((int)enumValue);

        return numberValues.Contains(status);
    }
}
