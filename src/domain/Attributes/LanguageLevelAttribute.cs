using domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes;

public class LanguageLevelAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (!int.TryParse(value.ToString(), out int level))
            return false;

        int[] levels =
        [
            (int)LagnuageLevel.A1,
            (int)LagnuageLevel.A2,
            (int)LagnuageLevel.B1,
            (int)LagnuageLevel.B2,
            (int)LagnuageLevel.C1,
            (int)LagnuageLevel.C2,
            (int)LagnuageLevel.Native
        ];

        return levels.Contains(level);
    }
}
