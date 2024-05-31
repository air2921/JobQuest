using domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes
{
    public class RoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null || value is not string)
                return false;

            string[] roles =
            {
                $"{Role.Candidate.ToString()}",
                $"{Role.Employer.ToString()}",
                $"{Role.Admin.ToString()}",
            };

            return roles.Contains((string)value);
        }
    }
}
