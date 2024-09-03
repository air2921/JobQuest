using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class PhoneTests
{
    private readonly PhoneNumberAttribute _attributeNullable = new(false);
    private readonly PhoneNumberAttribute _attributeNonNullable = new(true);

    public PhoneTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Fact]
    public void IsValid_NonNullable_Success()
    {
        var phone = "+1-234-567-89-01";
        var result = _attributeNonNullable.IsValid(phone);
        Assert.True(result);
    }

    [Theory]
    [InlineData("+123456789012")]
    [InlineData("123-456-789-01-23")]
    [InlineData("+12-345-678-901-23")]
    [InlineData("+123-456-789-12-345")]
    [InlineData("+12-345-abc-def-gh")]
    public void IsValid_NonNullable_Fail(string phone)
    {
        var result = _attributeNonNullable.IsValid(phone);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Nullable_Success()
    {
        string? phone = null;
        var result = _attributeNullable.IsValid(phone);
        Assert.True(result);
    }

    [Theory]
    [InlineData("+123456789012")]
    [InlineData("123-456-789-01-23")]
    [InlineData("+12-345-678-901-23")]
    [InlineData("+123-456-789-12-345")]
    [InlineData("+12-345-abc-def-gh")]
    public void IsValid_Nullable_Fail(string phone)
    {
        var result = _attributeNullable.IsValid(phone);
        Assert.False(result);
    }
}
