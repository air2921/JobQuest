﻿using domain.Enums;
using JsonLocalizer;
using domain.Localize;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RoleAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = Localizer.Translate(Validation.ROLE);

        if (value is null || value is not string stringValue)
            return false;

        var enumValues = Enum.GetValues(typeof(Role)).Cast<Role>();
        var stringValues = enumValues.Select(e => e.ToString()).ToList();

        return stringValues.Contains(stringValue);
    }
}
