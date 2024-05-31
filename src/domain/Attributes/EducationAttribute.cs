using domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes
{
    public class EducationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;

            if (!int.TryParse(value.ToString(), out int level))
                return false;

            int[] levels =
            [
                (int)EducationLevel.Secondary,
                (int)EducationLevel.Vocational,
                (int)EducationLevel.IncompleteHigher,
                (int)EducationLevel.Higher,
                (int)EducationLevel.Bachelor,
                (int)EducationLevel.Master,
                (int)EducationLevel.PhD,
                (int)EducationLevel.DoF,
            ];

            return levels.Contains(level);
        }
    }
}
