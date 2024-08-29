using Ardalis.Specification;
using domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Specifications.Response;

public class ResponseByIdSpec : IncludeSpec<ResponseModel>
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
