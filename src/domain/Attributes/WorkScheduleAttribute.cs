using domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Attributes
{
    public class WorkScheduleAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;

            if (!int.TryParse(value.ToString(), out int workSchedule))
                return false;

            int[] workSchedules =
            [
                (int)WorkSchedule.FullDay,
                (int)WorkSchedule.Remote,
                (int)WorkSchedule.Flexible
            ];

            return workSchedules.Contains(workSchedule);
        }
    }
}
