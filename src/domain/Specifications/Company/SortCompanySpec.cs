using Ardalis.Specification;
using domain.SpecDTO;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Company;

public class SortCompanySpec : SortCollectionSpec<CompanyModel>
{
    public SortCompanySpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.RegisterDate)
    {
        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.UserId.HasValue)
            Query.Where(x => x.UserId.Equals(DTO.UserId));

        if (!string.IsNullOrWhiteSpace(DTO.CompanyName))
        {
            Query.Where(x => x.CompanyName.Contains(DTO.CompanyName, StringComparison.InvariantCultureIgnoreCase));
            Query.OrderBy(x => Math.Abs(x.CompanyName.IndexOf(DTO.CompanyName) - x.CompanyName.Length));
        }

        if (DTO.CompanyGrade.HasValue)
            Query.Where(x => x.Reviews.Count > 0 &&
                x.Reviews.Sum(r => r.OverallGrade) >= DTO.CompanyGrade);

        if (DTO.Locations is not null && DTO.Locations.Any())
            Query.Where(x => DTO.Locations.Contains(x.Location));

        if (DTO.HasOpenedVacancies.HasValue)
        {
            Query.Where(x => x.Vacancies != null && x.Vacancies.Count > 0);

            if (DTO.HasOpenedVacancies.Value)
                Query.Where(x => x.Vacancies.Any(v => v.IsOpened));
            else
                Query.Where(x => x.Vacancies.All(v => !v.IsOpened));
        }

        Initialize();
    }

    public SortCompanyDTO? DTO { get; set; }
}
