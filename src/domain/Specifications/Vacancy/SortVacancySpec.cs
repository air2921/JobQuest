using Ardalis.Specification;
using domain.SpecDTO;
using domain.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace domain.Specifications.Vacancy;

public class SortVacancySpec : SortCollectionSpec<VacancyModel>
{
    public SortVacancySpec(int skip, int count, bool byDesc) 
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.CompanyId.HasValue)
            Query.Where(x => x.CompanyId.Equals(DTO.CompanyId));

        if (DTO.IsOpened.HasValue)
            Query.Where(x => x.IsOpened.Equals(DTO.IsOpened));

        if (DTO.Locations is not null && DTO.Locations.Any())
            Query.Where(x => DTO.Locations.Contains(x.Location));

        if (DTO.MinExperience.HasValue)
            Query.Where(x => x.Experience >= DTO.MinExperience.Value);

        if (DTO.MaxExperience.HasValue)
            Query.Where(x => x.Experience <= DTO.MaxExperience.Value);

        if (DTO.MinSalary.HasValue)
            Query.Where(x => x.MinSalary >= DTO.MinSalary.Value);

        if (DTO.MaxSalary.HasValue)
            Query.Where(x => x.MaxSalary <= DTO.MaxSalary.Value);

        if (DTO.EducationLevels is not null && DTO.EducationLevels.Any())
            Query.Where(x => DTO.EducationLevels.Contains(x.EducationLevel));

        if (DTO.Employments is not null && DTO.Employments.Any())
            Query.Where(x => DTO.Employments.Contains(x.Employment));

        if (DTO.WorkSchedules is not null && DTO.WorkSchedules.Any())
            Query.Where(x => DTO.WorkSchedules.Contains(x.WorkSchedule));

        if (DTO.Name is not null)
        {
            Query.Where(x => x.VacancyName.Contains(DTO.Name, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(x => Math.Abs(x.VacancyName.IndexOf(DTO.Name) - x.VacancyName.Length));
        }

        if (!string.IsNullOrEmpty(DTO.KeyWord))
        {
            DTO.KeyWord = DTO.KeyWord.ToLowerInvariant();
            var pattern = $@"\b{Regex.Escape(DTO.KeyWord)}\b";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            Query.Where(x =>
                regex.IsMatch(x.About) ||
                regex.IsMatch(x.VacancyName)
            );

            Query.OrderByDescending(x => CountKeywordOccurrences(x.About, DTO.KeyWord))
                .ThenBy(x => Math.Abs(x.VacancyName.IndexOf(DTO.KeyWord, StringComparison.InvariantCultureIgnoreCase) - x.VacancyName.Length));
        }

        Initialize();
    }

    public SortVacancyDTO? DTO { get; set; }

    private static int CountKeywordOccurrences(string text, string keyword)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
            return 0;

        var regex = new Regex(Regex.Escape(keyword), RegexOptions.IgnoreCase);
        return regex.Matches(text).Count;
    }
}
