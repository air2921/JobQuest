using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class ResponseTests
{
    private readonly ResponseStatusAttribute _attribute = new();

    public ResponseTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData((int)StatusResponse.Refusal)]
    [InlineData((int)StatusResponse.Invitation)]
    [InlineData((int)StatusResponse.Expectation)]
    public void IsValid_Success(int status)
    {
        var result = _attribute.IsValid(status);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Fail()
    {
        var status = 999;
        var result = _attribute.IsValid(status);
        Assert.False(result);
    }
}
