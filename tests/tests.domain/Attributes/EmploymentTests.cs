using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class EmploymentTests
{
    private readonly EmploymentAttribute _attribute = new();

    public EmploymentTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData((int)Employment.Full)]
    [InlineData((int)Employment.Project)]
    [InlineData((int)Employment.Internship)]
    [InlineData((int)Employment.Partial)]
    public void IsValid_Success(int employment)
    {
        var result = _attribute.IsValid(employment);
        Assert.True(result);
    }

    [Theory]
    [InlineData(999)]
    public void IsValid_Fail(int employment)
    {
        var result = _attribute.IsValid(employment);
        Assert.False(result);
    }
}
