using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class RoleTests
{
    private readonly RoleAttribute _attribute = new();

    public RoleTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("Employer")]
    [InlineData("Applicant")]
    public void IsValid_Success(string role)
    {
        var result = _attribute.IsValid(role);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Fail()
    {
        var role = "None";
        var result = _attribute.IsValid(role);
        Assert.False(result);
    }
}
