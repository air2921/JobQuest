using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Response;

public class ResponseByIdSpec : IncludeSpec<ResponseModel>, IEntityById<ResponseModel>
{
    public ResponseByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ResponseId.Equals(Id));

        if (Expressions is not null && Expressions.Any())
            IncludeEntities(Expressions);
    }

    public int Id { get; set; }
}
