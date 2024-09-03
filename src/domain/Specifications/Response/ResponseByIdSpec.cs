using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Response;

public class ResponseByIdSpec : Specification<ResponseModel>, IEntityById<ResponseModel>
{
    public ResponseByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.ResponseId.Equals(Id));
    }

    public int Id { get; set; }
}
