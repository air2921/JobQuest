using Ardalis.Specification;
using domain.Localize;
using domain.Models;
using domain.SpecDTO;
using JsonLocalizer;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace domain.Specifications.Response;

public class SortResponseSpec : SortCollectionSpec<ResponseModel>
{
    public SortResponseSpec(int skip, int count, bool byDesc, bool asEmployer)
        : base(skip, count, byDesc, x => x.ResponseOfDate)
    {
        if (AsEmployer)
        {
            if (VacancyId is null)
                throw new ValidationException(Localizer.Translate(Messages.INVALID_SORT));

            Query.Where(x => x.VacancyId.Equals(VacancyId));
        }
        else
        {
            if (ResumeId is null)
                throw new ValidationException(Localizer.Translate(Messages.INVALID_SORT));

            Query.Where(x => x.ResumeId.Equals(ResumeId));
        }

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

    public int? ResumeId { get; set; }
    public int? VacancyId { get; set; }
    public bool AsEmployer { get; set; }
    public SortResponseDTO? DTO { get; set; }
}
