using domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class WorkScheduleAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int workSchedule))
            return false;

        var enumValues = Enum.GetValues(typeof(WorkSchedule));
        var numberValues = new List<int>();

        foreach (var enumValue in enumValues)
            numberValues.Add((int)enumValue);

        return numberValues.Contains(workSchedule);
    }
}
