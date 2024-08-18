using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class EmailAttribute(bool nullValidate) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return !nullValidate;

        if (value is not string email)
            return false;

        string pattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
        Regex regex = new(pattern);

        return regex.IsMatch(email) && !HasUpperCase(email);
    }

    private static bool HasUpperCase(string str)
    {
        foreach (char c in str)
            if (char.IsUpper(c))
                return true;

        return false;
    }
}
