using System.Text;

namespace domain.SpecDTO;

public class PaginationDTO
{
    public int Skip { get; set; } = 0;
    public int Total { get; set; } = 50;
    public bool ByDesc { get; set; } = true;

    public override string ToString()
    {
        var builder = new StringBuilder(50);
        builder.Append('-');
        builder.Append(Skip);
        builder.Append('-');
        builder.Append(Total);
        builder.Append('-');
        builder.Append(ByDesc);
        return builder.ToString();
    }
}
