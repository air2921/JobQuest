using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Education;

public class EducationByIdSpec : Specification<EducationModel>, IEntityById<EducationModel>
{
    public EducationByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.EducationId.Equals(Id));
    }

    public int Id { get; set; }
}
