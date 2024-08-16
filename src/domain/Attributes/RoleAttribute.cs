using domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RoleAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null || value is not string)
            return false;

        string[] roles =
        [
            $"{Role.Candidate}",
            $"{Role.Employer}",
            $"{Role.Admin}",
        ];

        return roles.Contains((string)value);
    }
}
