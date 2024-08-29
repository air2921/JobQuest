using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Resume;

public class ResumeByIdSpec : IncludeSpec<ResumeModel>, IEntityById<ResumeModel>
{
    public ResumeByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ResumeId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
