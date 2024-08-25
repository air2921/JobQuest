using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NameAttribute(bool nullValidate) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return !nullValidate;

        if (value is not string name)
            return false;

        string pattern = @"[^ \p{L}]";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

        return regex.IsMatch(name);
    }
}
