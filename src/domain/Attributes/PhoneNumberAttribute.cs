﻿using System;
using JsonLocalizer;
using domain.Localize;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PhoneNumberAttribute(bool nullValidate) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = Localizer.Translate(Validation.PHONE);

        if (value is null)
            return !nullValidate;

        if (value is not string phoneNumber)
            return false;

        var pattern = @"^\+\d{1,9}-\d{3}-\d{3}-\d{2}-\d{2}$";
        return Regex.IsMatch(phoneNumber, pattern);
    }
}
