using Ardalis.Specification;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace domain.Specifications.Vacancy;

public class SortVacancySpec : SortCollectionSpec<VacancyModel>
{
    public SortVacancySpec(int skip, int count, bool byDesc) : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (CompanyId.HasValue)
            Query.Where(x => x.CompanyId.Equals(CompanyId));

        if (IsOpened.HasValue)
            Query.Where(x => x.IsOpened.Equals(IsOpened));

        if (Locations is not null && Locations.Any())
            Query.Where(x => Locations.Contains(x.Location));

        if (MinExperience.HasValue)
            Query.Where(x => x.Experience >= MinExperience.Value);

        if (MaxExperience.HasValue)
            Query.Where(x => x.Experience <= MaxExperience.Value);

        if (MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= MinSalary.Value);

        if (MaxSalary.HasValue)
            Query.Where(x => x.MaxSalary <= MaxSalary.Value);

        if (EducationLevels is not null && EducationLevels.Any())
            Query.Where(x => EducationLevels.Contains(x.EducationLevel));

        if (Employments is not null && Employments.Any())
            Query.Where(x => Employments.Contains(x.Employment));

        if (WorkSchedules is not null && WorkSchedules.Any())
            Query.Where(x => WorkSchedules.Contains(x.WorkSchedule));

        if (Name is not null)
        {
            Query.Where(x => x.VacancyName.Contains(Name));
            Query.OrderBy(x => Math.Abs(x.VacancyName.IndexOf(Name) - x.VacancyName.Length));
        }

        Initialize();
    }

    public int? CompanyId { get; set; }
    public bool? IsOpened { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }
    public int? MinExperience { get; set; }
    public int? MaxExperience { get; set; }
    public IEnumerable<int>? EducationLevels { get; set; }
    public IEnumerable<int>? Employments { get; set; }
    public IEnumerable<int>? WorkSchedules { get; set; }
    public string? Name { get; set; }
}
