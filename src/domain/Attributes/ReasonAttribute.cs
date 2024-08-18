using domain.Enums;
using System;
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

        int[] reasons =
        [
            (int)Reason.SalaryLevelInsufficient,
            (int)Reason.InsufficientQualifications,
            (int)Reason.Unreliability,
            (int)Reason.Dishonesty,
            (int)Reason.PoorReferences,
            (int)Reason.FrequentJobChanges,
            (int)Reason.NotMeetingVacancyRequirements,
            (int)Reason.CulturalMisfit,
            (int)Reason.LackOfRelevantExperience,
            (int)Reason.LackOfInterestOrMotivation,
            (int)Reason.VacancyClosed,
            (int)Reason.Other
        ];

        return reasons.Contains(reason);
    }
}
