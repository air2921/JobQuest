using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace domain.SpecDTO;

public class SortCompanyDTO : PaginationDTO
{
    public double? CompanyGrade { get; set; }
    public int? UserId { get; set; }
    public string? CompanyName { get; set; }
    public bool? HasOpenedVacancies { get; set; }
    public IEnumerable<string>? Locations { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder(256);
        builder.Append(UserId.ToString() ?? "null");
        builder.Append('-');
        builder.Append(CompanyGrade.HasValue ? CompanyGrade : "null");
        builder.Append('-');
        builder.Append(CompanyName ?? "null");
        builder.Append('-');
        builder.Append(HasOpenedVacancies.ToString() ?? "null");
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
