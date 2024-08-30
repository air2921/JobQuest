using Ardalis.Specification;
using domain.Models;
using domain.SpecDTO;
using System.Linq;

namespace domain.Specifications.Response;

public class SortResponseSpec : SortCollectionSpec<ResponseModel>
{
    public SortResponseSpec(int skip, int count, bool byDesc, int resumeId)
        : base(skip, count, byDesc, x => x.ResponseOfDate)
    {
        ResumeId = resumeId;

        Query.Where(x => x.ResponseId.Equals(ResumeId));

        if (DTO is null)
        {
            Initialize();
            return;
        }

        if (DTO.Status.HasValue)
            Query.Where(x => x.Status.Equals(DTO.Status));

        if (DTO.Reason.HasValue)
            Query.Where(x => x.Reason.Equals(DTO.Reason));

        if (DTO.HasDescription.HasValue)
            Query.Where(x => DTO.HasDescription.Value
                ? !string.IsNullOrWhiteSpace(x.ReasonDescription)
                : string.IsNullOrWhiteSpace(x.ReasonDescription));

        Initialize();
    }

    public int ResumeId { get; private set; }
    public SortResponseDTO? DTO { get; set; }
}
