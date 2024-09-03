using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class UrlTests
{
    private readonly UrlAttribute _attributeNullable = new(false);
    private readonly UrlAttribute _attributeNonNullable = new(true);

    public UrlTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Fact]
    public void IsValid_NonNullable_Success()
    {
        var url = "https://github.com/air2921";
        var result = _attributeNonNullable.IsValid(url);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_NonNullable_Fail()
    {
        var url = "github.com/air2921";
        var result = _attributeNonNullable.IsValid(url);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Nullable_Success()
    {
        string? url = null;
        var result = _attributeNullable.IsValid(url);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Nullable_Fail()
    {
        var url = "github.com/air2921";
        var result = _attributeNullable.IsValid(url);
        Assert.False(result);
    }
}
