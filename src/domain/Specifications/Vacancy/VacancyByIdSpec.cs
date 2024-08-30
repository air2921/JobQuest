using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Vacancy;

public class VacancyByIdSpec : IncludeSpec<VacancyModel>, IEntityById<VacancyModel>
{
    public VacancyByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.VacancyId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
