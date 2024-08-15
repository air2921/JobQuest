using domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

public class EmploymentAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int employment))
            return false;

        int[] employments =
        [
            (int)Employment.Full,
            (int)Employment.Partial,
            (int)Employment.Internship,
            (int)Employment.Project
        ];

        return employments.Contains(employment);
    }
}
