using domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Attributes
{
    public class ResponseStatusAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;

            if (!int.TryParse(value.ToString(), out int status))
                return false;

            int[] statuses =
            [
                (int)StatusResponse.Expectation,
                (int)StatusResponse.Invitation,
                (int)StatusResponse.Refusal,
            ];

            return statuses.Contains(status);
        }
    }
}
