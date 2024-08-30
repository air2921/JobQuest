using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Company;

public class CompanyByRelationSpec : IncludeSpec<CompanyModel>, IEntityById<CompanyModel>
{
    public CompanyByRelationSpec(int userId)
    {
        Id = userId;

        Query.Where(x => x.UserId.Equals(Id));
    }

    public int Id { get; set; }
}
