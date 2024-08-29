using Ardalis.Specification;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Specifications.Vacancy;

public class VacancyByIdSpec : IncludeSpec<VacancyModel>
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
