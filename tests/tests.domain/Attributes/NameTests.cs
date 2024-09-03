using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class NameTests
{
    private readonly NameAttribute _attributeNullable = new(false);
    private readonly NameAttribute _attributeNonNullable = new(true);

    public NameTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Fact]
    public void IsValid_NonNullable_Success()
    {
        var name = "John";
        var result = _attributeNonNullable.IsValid(name);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_NonNullable_Fail()
    {
        var name = "John123";
        var result = _attributeNonNullable.IsValid(name);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Nullable_Success()
    {
        string? name = null;
        var result = _attributeNullable.IsValid(name);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Nullable_Fail()
    {
        var name = "John123";
        var result = _attributeNullable.IsValid(name);
        Assert.False(result);
    }
}
