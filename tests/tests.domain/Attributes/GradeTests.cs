using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class GradeTests
{
    private readonly GradeAttribute _attribute = new();

    public GradeTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void IsValid_Success(int education)
    {
        var result = _attribute.IsValid(education);
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void IsValid_Fail(int education)
    {
        var result = _attribute.IsValid(education);
        Assert.False(result);
    }
}
