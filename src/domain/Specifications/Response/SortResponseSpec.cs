using Ardalis.Specification;
using domain.Models;
using System.Linq;

namespace domain.Specifications.Response;

public class SortResponseSpec : SortCollectionSpec<ResponseModel>
{
    public SortResponseSpec(int skip, int count, bool byDesc, int resumeId)
        : base(skip, count, byDesc, x => x.ResponseOfDate)
    {
        ResumeId = resumeId;

        Query.Where(x => x.ResponseId.Equals(ResumeId));

        if (Status.HasValue)
            Query.Where(x => x.Status.Equals(Status.Value));

        Initialize();
    }

    public int ResumeId { get; set; }
    public int? Status { get; set; }
}
