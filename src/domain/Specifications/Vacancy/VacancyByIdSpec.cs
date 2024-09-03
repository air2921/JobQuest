using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Vacancy;

public class VacancyByIdSpec : Specification<VacancyModel>, IEntityById<VacancyModel>
{
    public VacancyByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.VacancyId.Equals(Id));
    }

    public int Id { get; set; }
}
