using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace domain.SpecDTO;

public class SortExperienceDTO : PaginationDTO
{
    public int? ResumeId { get; set; }
    public IEnumerable<string>? SpecialityNames { get; set; }
    public IEnumerable<string>? Companies { get; set; }
    public IEnumerable<string>? Locations { get; set; }
    public bool? HasWebSite { get; set; }
    public bool? HasDuties { get; set; }
    public bool? IsPresentTime { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder(1024);

        builder.Append(ResumeId.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasWebSite.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasDuties.ToString() ?? "null");
        builder.Append('-');

        builder.Append(IsPresentTime.ToString() ?? "null");
        builder.Append('-');

        if (SpecialityNames is not null && SpecialityNames.Any())
        {
            foreach (var name in SpecialityNames)
            {
                builder.Append(name ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Companies is not null && Companies.Any())
        {
            foreach (var company in Companies)
            {
                builder.Append(company ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

        if (Locations is not null && Locations.Any())
        {
            foreach (var location in Locations)
            {
                builder.Append(location ?? "null");
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
