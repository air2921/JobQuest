using domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class JobStatusAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int status))
            return false;

        int[] statuses =
        [
            (int)JobStatus.Actively,
            (int)JobStatus.Сonsidering,
            (int)JobStatus.Think,
            (int)JobStatus.Accepted,
            (int)JobStatus.NoSearching
        ];

        return statuses.Contains(status);
    }
}
