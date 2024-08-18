using domain.Models;

namespace domain.Specifications.Resume;

public class SortResumeSpec : SortCollectionSpec<ResumeModel>
{
    public SortResumeSpec(int skip, int count, bool byDesc)
        : base(skip, count, byDesc, x => x.CreatedAt)
    {
        
    }
}
