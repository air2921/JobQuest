using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Vacancy
{
    public class VacancyByCompanyNameSpec : Specification<VacancyModel>
    {
        public VacancyByCompanyNameSpec(string companyName)
        {
            CompanyName = companyName;

            Query.Where(x => x.CompanyName.Equals(CompanyName));
        }

        public string CompanyName { get; set; }
    }
}
