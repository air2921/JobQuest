using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Vacancy;

public class SortVacancySpec : Specification<VacancyModel>
{
    public SortVacancySpec(string[]? locations, int? minSal,int? minExp,
        int[]? educLevels, int[]? employments, int[]? schedules,
        string? name, bool byDesc, int skip, int count)
    {
        Locations = locations;
        MinSalary = minSal;
        MinExperience = minExp;
        EducationLevels = educLevels;
        Employments = employments;
        WorkSchedules = schedules;
        Name = name;
        ByDesc = byDesc;
        SkipCount = skip;
        Count = count;

        if (Locations is not null && Locations.Length > 0)
            Query.Where(x => Locations.Contains(x.Location));

        if (MinExperience.HasValue)
            Query.Where(x => x.Experience >= MinExperience.Value);

        if (MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= MinSalary.Value);

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

        if (ByDesc)
            Query.OrderByDescending(x => x.CreatedAt);
        else
            Query.OrderBy(x => x.CreatedAt);

        Query.Skip(SkipCount).Take(Count);
    }

    public string[]? Locations { get; private set; }
    public int? MinSalary { get; private set; }
    public int? MinExperience { get; private set; }
    public int[]? EducationLevels { get; private set; }
    public int[]? Employments { get; private set; }
    public int[]? WorkSchedules { get; private set; }
    public string? Name { get; private set; }
    public bool ByDesc { get; private set; }
    public int SkipCount { get; private set; }
    public int Count { get; private set; }
}
