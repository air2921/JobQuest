using Ardalis.Specification;
using domain.Models;
using System;

namespace domain.Specifications.Review
{
    public class SortReviewSpec : Specification<ReviewModel>
    {
        public SortReviewSpec(string? title, int companyId, bool? isRecomended,
            bool byDesc, int skipCount, int count)
        {
            Title = title;
            CompanyId = companyId;
            IsRecomended = isRecomended;
            ByDesc = byDesc;
            SkipCount = skipCount;
            Count = count;

            Query.Where(x => x.CompanyId.Equals(CompanyId));

            if (IsRecomended.HasValue)
                Query.Where(x => x.IsRecomended.Equals(IsRecomended.Value));

            if (Title is not null)
            {
                Query.Where(x => x.JobTitle.Contains(Title));
                Query.OrderBy(x => Math.Abs(x.JobTitle.IndexOf(Title) - x.JobTitle.Length));
            }

            if (ByDesc)
                Query.OrderByDescending(x => x.CreatedAt);
            else
                Query.OrderBy(x => x.CreatedAt);

            Query.Skip(SkipCount).Take(Count);
        }

        public string? Title { get; private set; }
        public int CompanyId { get; private set; }
        public bool? IsRecomended { get; private set; }
        public bool ByDesc { get; private set; }
        public int SkipCount { get; private set; }
        public int Count { get; private set; }
    }
}
