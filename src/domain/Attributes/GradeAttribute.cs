using System.ComponentModel.DataAnnotations;

namespace domain.Attributes
{
    public class GradeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;

            if (!int.TryParse(value.ToString(), out int grade))
                return false;

            if (grade <= 0 || grade >= 6)
                return false;

            return true;
        }
    }
}
