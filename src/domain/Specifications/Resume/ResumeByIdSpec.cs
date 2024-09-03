using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Resume;

public class ResumeByIdSpec : Specification<ResumeModel>, IEntityById<ResumeModel>
{
    public ResumeByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ResumeId.Equals(Id));
    }

    public int Id { get; set; }
}
