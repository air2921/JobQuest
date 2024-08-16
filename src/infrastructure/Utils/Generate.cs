using domain.Abstractions;
using System;
using System.Text;

namespace infrastructure.Utils;

public class Generate : IGenerate
{
    public string GuidCombine(int count, bool useNoHyphensFormat = false)
    {
        if (count.Equals(0) || count >= 11)
            throw new NotSupportedException("Too long Guid");

        var builder = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            if (useNoHyphensFormat)
                builder.Append(Guid.NewGuid().ToString("N"));
            else
                builder.Append(Guid.NewGuid().ToString());
        }

        return builder.ToString();
    }

    public int GenerateCode(int length)
    {
        var rnd = new Random();

        if (length <= 0 || length >= 11)
            throw new NotSupportedException("Invalid code length");

        var builder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
            builder.Append(rnd.Next(10));

        return int.Parse(builder.ToString());
    }
}
