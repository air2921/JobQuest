using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Vacancy;

public class SortVacancySpec : SortCollectionSpec<VacancyModel>
{
    public SortVacancySpec(int skip, int count, bool byDesc) : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (Locations is not null && Locations.Length > 0)
            Query.Where(x => Locations.Contains(x.Location));

        if (MinExperience.HasValue)
            Query.Where(x => x.Experience >= MinExperience.Value);

        if (MaxExperience.HasValue)
            Query.Where(x => x.Experience <= MaxExperience.Value);

        if (MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= MinSalary.Value);

        if (MaxSalary.HasValue)
            Query.Where(x => x.MaxSalary <= MaxSalary.Value);

        if (EducationLevels is not null && EducationLevels.Length > 0)
            Query.Where(x => EducationLevels.Contains(x.EducationLevel));

        if (Employments is not null && Employments.Length > 0)
            Query.Where(x => Employments.Contains(x.Employment));

        if (WorkSchedules is not null && WorkSchedules.Length > 0)
            Query.Where(x => WorkSchedules.Contains(x.WorkSchedule));

        if (Name is not null)
        {
            Query.Where(x => x.VacancyName.Contains(Name));
            Query.OrderBy(x => Math.Abs(x.VacancyName.IndexOf(Name) - x.VacancyName.Length));
        }

        Initialize();
    }

    public string[]? Locations { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public int[]? EducationLevels { get; set; }
    public int[]? Employments { get; set; }
    public int[]? WorkSchedules { get; set; }
    public string? Name { get; set; }
}
