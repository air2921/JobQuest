using Ardalis.Specification;
using domain.Models;
using System;
using System.Linq;

namespace domain.Specifications.Company;

public class CompanyByIdSpec : IncludeSpec<CompanyModel>
{
    public CompanyByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.CompanyId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
