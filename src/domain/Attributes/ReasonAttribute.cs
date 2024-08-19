using domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ReasonAttribute(bool nullValidate) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return !nullValidate;

        if (!int.TryParse(value.ToString(), out int reason))
            return false;

        var enumValues = Enum.GetValues(typeof(Reason));
        var numberValues = new List<int>();

        foreach (var enumValue in enumValues)
            numberValues.Add((int)enumValue);

        return numberValues.Contains(reason);
    }
}
