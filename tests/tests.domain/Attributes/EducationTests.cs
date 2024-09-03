using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class EducationTests
{
    private readonly EducationAttribute _attribute = new();

    public EducationTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData((int)EducationLevel.None)]
    [InlineData((int)EducationLevel.Secondary)]
    [InlineData((int)EducationLevel.Vocational)]
    [InlineData((int)EducationLevel.IncompleteHigher)]
    [InlineData((int)EducationLevel.Higher)]
    [InlineData((int)EducationLevel.Bachelor)]
    [InlineData((int)EducationLevel.Master)]
    [InlineData((int)EducationLevel.PhD)]
    [InlineData((int)EducationLevel.DoF)]
    public void IsValid_Success(int education)
    {
        var result = _attribute.IsValid(education);
        Assert.True(result);
    }

    [Theory]
    [InlineData(999)]
    public void IsValid_Fail(int education)
    {
        var result = _attribute.IsValid(education);
        Assert.False(result);
    }
}
