using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.SpecDTO;

public class SortLanguageDTO : PaginationDTO
{
    public string[]? Languages { get; set; }
    public int[]? Levels { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder(256);

        if (Languages is not null && Languages.Any())
        {
            foreach (var language in Languages)
            {
                builder.Append(language ?? "null");
                builder.Append('-');
            }
        }
        else
        {
            builder.Append("null");
            builder.Append('-');
        }

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

        builder.Append(Skip);
        builder.Append('-');
        builder.Append(Total);
        builder.Append('-');
        builder.Append(ByDesc);

        return builder.ToString();
    }
}
