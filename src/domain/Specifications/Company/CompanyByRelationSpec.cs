using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Company;

public class CompanyByRelationSpec : IncludeSpec<CompanyModel>
{
    public CompanyByRelationSpec(int userId)
    {
        UserId = userId;

        Query.Where(x => x.UserId.Equals(UserId));
    }

    public int UserId { get; set; }
}
