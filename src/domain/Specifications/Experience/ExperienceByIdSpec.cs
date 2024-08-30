using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Experience;

public class ExperienceByIdSpec : IncludeSpec<ExperienceModel>, IEntityById<ExperienceModel>
{
    public ExperienceByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ExperienceId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
