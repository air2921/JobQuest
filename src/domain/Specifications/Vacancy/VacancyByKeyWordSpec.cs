using Ardalis.Specification;
using domain.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace domain.Specifications.Vacancy;

public class VacancyByKeyWordSpec : SortCollectionSpec<VacancyModel>
{
    public VacancyByKeyWordSpec(int skip, int count, bool orderByDesc, string keyWord)
        : base(skip, count, orderByDesc, x => x.CreatedAt)
    {
        KeyWord = keyWord.ToLowerInvariant();

        if (!string.IsNullOrEmpty(KeyWord))
        {
            var pattern = $@"\b{Regex.Escape(KeyWord)}\b";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            Query.Where(x =>
                regex.IsMatch(x.About) ||
                regex.IsMatch(x.VacancyName)
            );
        }

        Initialize();
    }

    public string KeyWord { get; private set; }
}
