using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.SpecDTO;

public class SortResponseDTO : PaginationDTO
{
    public int? Status { get; set; }
    public int? Reason { get; set; }
    public bool? HasDescription { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder(128);

        builder.Append(Status.ToString() ?? "null");
        builder.Append('-');

        builder.Append(Reason.ToString() ?? "null");
        builder.Append('-');

        builder.Append(HasDescription.ToString() ?? "null");
        builder.Append('-');

        builder.Append(Skip);
        builder.Append('-');
        builder.Append(Total);
        builder.Append('-');
        builder.Append(ByDesc);

        return builder.ToString();
    }
}
