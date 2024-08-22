using Ardalis.Specification;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace domain.Specifications.Company;

public class SortCompanySpec : SortCollectionSpec<CompanyModel>
{
    public SortCompanySpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.RegisterDate)
    {
        if (UserId.HasValue)
            Query.Where(x => x.UserId.Equals(UserId));

        if (!string.IsNullOrWhiteSpace(CompanyName))
        {
            Query.Where(x => x.CompanyName.Contains(CompanyName));
            Query.OrderBy(x => Math.Abs(x.CompanyName.IndexOf(CompanyName) - x.CompanyName.Length));
        }

        if (CompanyGrade.HasValue)
            Query.Where(x => x.Reviews != null && x.Reviews.Count > 0 &&
                x.Reviews.Sum(r => r.OverallGrade) >= CompanyGrade);

        if (Locations is not null && Locations.Any())
            Query.Where(x => Locations.Contains(x.Location));

        if (HasOpenedVacancies.HasValue)
        {
            Query.Where(x => x.Vacancies != null && x.Vacancies.Count > 0);

            if (HasOpenedVacancies.Value)
                Query.Where(x => x.Vacancies.Any(v => v.IsOpened));
            else
                Query.Where(x => x.Vacancies.All(v => !v.IsOpened));
        }

        Initialize();
    }

    public double? CompanyGrade { get; set; }
    public int? UserId { get; set; }
    public string? CompanyName { get; set; }
    public bool? HasOpenedVacancies { get; set; }
    public IEnumerable<string>? Locations { get; set; }
}
