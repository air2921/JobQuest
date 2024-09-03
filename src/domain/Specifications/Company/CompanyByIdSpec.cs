using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Company;

public class CompanyByIdSpec : Specification<CompanyModel>, IEntityById<CompanyModel>
{
    public CompanyByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.CompanyId.Equals(Id));
    }

    public int Id { get; set; }
}
