using domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class EmploymentAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int employment))
            return false;

        var enumValues = Enum.GetValues(typeof(Employment));
        var numberValues = new List<int>();

        foreach (var enumValue in enumValues)
            numberValues.Add((int)enumValue);

        return numberValues.Contains(employment);
    }
}
