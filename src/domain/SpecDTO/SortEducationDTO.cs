using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace domain.SpecDTO;

public class SortEducationDTO : PaginationDTO
{
    public int? ResumeId { get; set; }
    public IEnumerable<int>? Levels { get; set; }
    public IEnumerable<string>? Institutions { get; set; }
    public IEnumerable<string>? Specialties { get; set; }
    public bool? StillStyding { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder(1024);

        builder.Append(ResumeId.ToString() ?? "null");
        builder.Append('-');
        builder.Append(StillStyding.ToString() ?? "null");

        if (Levels is not null && Levels.Any())
        {
            foreach (var level in Levels)
            {
                builder.Append(level.ToString() ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Institutions is not null && Institutions.Any())
        {
            foreach (var institution in Institutions)
            {
                builder.Append(institution ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Specialties is not null && Specialties.Any())
        {
            foreach (var specialty in Specialties)
            {
                builder.Append(specialty ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        builder.Append(Skip);
        builder.Append('-');
        builder.Append(Total);
        builder.Append('-');
        builder.Append(ByDesc);

        return builder.ToString();
    }
}
