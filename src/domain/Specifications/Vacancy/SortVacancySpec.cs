using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Vacancy
{
    public class SortVacancySpec : Specification<VacancyModel>
    {
        public SortVacancySpec(string[]? locations, int? minSal, int? maxSal,
            int? minExp, int? maxExp, int[]? educLevels, int[]? employments, int[]? schedules)
        {
            Locations = locations;
            MinSalary = minSal;
            MaxSalary = maxSal;
            MinExperience = minExp;
            MaxExperience = maxExp;
            EducationLevels = educLevels;
            Employments = employments;
            WorkSchedules = schedules;

            if (Locations is not null && Locations.Length > 0)
                Query.Where(x => Locations.Contains(x.Location));

            if (MinExperience.HasValue && MaxExperience.HasValue)
                Query.Where(x => x.Experience >= MinExperience.Value && x.Experience <= MaxExperience.Value);

            if (MinSalary.HasValue && MaxSalary.HasValue)
                Query.Where(x => x.MinSalary >= MinSalary.Value && x.MaxSalary <= MaxSalary.Value);

            if (EducationLevels is not null && EducationLevels.Length > 0)
                Query.Where(x => EducationLevels.Contains(x.EducationLevel));

            if (Employments is not null && Employments.Length > 0)
                Query.Where(x => Employments.Contains(x.Employment));

            if (WorkSchedules is not null && WorkSchedules.Length > 0)
                Query.Where(x => WorkSchedules.Contains(x.WorkSchedule));
        }

        public string[]? Locations { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public int? MinExperience { get; set; }
        public int? MaxExperience { get; set; }
        public int[]? EducationLevels { get; set; }
        public int[]? Employments { get; set; }
        public int[]? WorkSchedules { get; set; }
    }
}
