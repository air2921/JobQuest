using domain.Abstractions;
using System;
using System.Text;

namespace infrastructure.Utils;

public class Generate : IGenerate
{
    private static readonly Random _rnd = new();

    private const int MIN = 0;
    private const int MAX = 10;

    public string GuidCombine(int count, bool useNoHyphensFormat = false)
    {
        if (count <= MIN || count >= MAX)
            throw new NotSupportedException($"Combine of {count} guid is not supported");

        var builder = new StringBuilder(useNoHyphensFormat ? 32 : 36 * count);
        for (int i = 0; i < count; i++)
            builder.Append(Guid.NewGuid().ToString(useNoHyphensFormat ? "N" : string.Empty));

        return builder.ToString();
    }

    public int GenerateCode(int length)
    {
        if (length <= MIN || length >= MAX)
            throw new NotSupportedException($"Lenght {length} is not supported");

        var builder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
            builder.Append(_rnd.Next(10));

        return int.Parse(builder.ToString());
    }
}
