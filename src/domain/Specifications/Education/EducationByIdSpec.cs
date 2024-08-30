using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Education;

public class EducationByIdSpec : IncludeSpec<EducationModel>, IEntityById<EducationModel>
{
    public EducationByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.EducationId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
