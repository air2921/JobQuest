using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes
{
    public class HashAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null || value is not string)
                return false;

            string passwordHash = (string)value;
            string bcryptPattern = @"^\$2[ayb]\$.{56}$";

            return Regex.IsMatch(passwordHash, bcryptPattern);
        }
    }
}
