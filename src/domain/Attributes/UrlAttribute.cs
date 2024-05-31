using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace domain.Attributes
{
    public class UrlAttribute(bool nullValidate) : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return !nullValidate;

            if (string.IsNullOrWhiteSpace(value.ToString()))
                return false;

            var urlPattern = @"^(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
            return Regex.IsMatch(value.ToString()!, urlPattern);
        }
    }
}
