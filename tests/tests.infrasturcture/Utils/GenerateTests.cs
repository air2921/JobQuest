using infrastructure.Utils;

namespace tests.infrasturcture.Utils;

public class GenerateTests
{
    private const int HYPHENS_FORMAT = 36;
    private const int NO_HYPHENS_FORMAT = 32;

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void GuidCombine_No_Hyphens_Format_Success(int count)
    {
        var generate = new Generate();
        var guid = generate.GuidCombine(count, true);

        Assert.Equal(NO_HYPHENS_FORMAT * count, guid.Length);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void GuidCombine_Hyphens_Format_Success(int count)
    {
        var generate = new Generate();
        var guid = generate.GuidCombine(count, false);

        Assert.Equal(HYPHENS_FORMAT * count, guid.Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void GuidCombine_Fail_Unsupported_Length(int count)
    {
        var generate = new Generate();
        Assert.Throws<NotSupportedException>(() => generate.GuidCombine(count));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void GenerateCode_Success(int length)
    {
        var generate = new Generate();
        var code = generate.GenerateCode(length);

        Assert.Equal(length, code.ToString().Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void GenerateCode_Fail_Unsupported_Length(int length)
    {
        var generate = new Generate();
        Assert.Throws<NotSupportedException>(() => generate.GenerateCode(length));
    }
}
