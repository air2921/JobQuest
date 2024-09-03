using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class EmailTests
{
    private readonly EmailAttribute _attributeNullable = new(false);
    private readonly EmailAttribute _attributeNonNullable = new(true);

    public EmailTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Fact]
    public void IsValid_NonNullable_Success()
    {
        var email = "johndoe134@gmail.com";
        var result = _attributeNonNullable.IsValid(email);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_NonNullable_Fail_HasUpperCharacters()
    {
        var email = "JohnDoe134@gmail.com";
        var result = _attributeNonNullable.IsValid(email);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_NonNullable_Fail()
    {
        var email = "JohnDoe";
        var result = _attributeNonNullable.IsValid(email);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Nullable_Success()
    {
        string? email = null;
        var result = _attributeNullable.IsValid(email);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Nullable_Fail()
    {
        var email = "JohnDoe";
        var result = _attributeNullable.IsValid(email);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Nullable_Fail_HasUpperCharacters()
    {
        var email = "JohnDoe134@gmail.com";
        var result = _attributeNullable.IsValid(email);
        Assert.False(result);
    }
}
