using Ardalis.Specification;
using domain.Abstractions;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Recovery;

public class RecoveryByIdSpec : Specification<RecoveryModel>, IEntityById<RecoveryModel>
{
    public RecoveryByIdSpec(int id)
    {
        Id = id;
        Query.Where(x => x.TokenId.Equals(Id));
    }

    public int Id { get; set; }
}
