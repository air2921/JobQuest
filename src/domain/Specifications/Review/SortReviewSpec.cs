using Ardalis.Specification;
using domain.Models;
using System;

namespace domain.Specifications.Review;

public class SortReviewSpec : SortCollectionSpec<ReviewModel>
{
    public SortReviewSpec(int skip, int count, bool byDesc, int companyId)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        CompanyId = companyId;

        Query.Where(x => x.CompanyId.Equals(CompanyId));

        if (IsRecomended.HasValue)
            Query.Where(x => x.IsRecomended.Equals(IsRecomended.Value));

        if (Title is not null)
        {
            Query.Where(x => x.JobTitle.Contains(Title));
            Query.OrderBy(x => Math.Abs(x.JobTitle.IndexOf(Title) - x.JobTitle.Length));
        }

        Initialize();
    }

    public string? Title { get; set; }
    public bool? IsRecomended { get; set; }
    public int CompanyId { get; private set; }
}
