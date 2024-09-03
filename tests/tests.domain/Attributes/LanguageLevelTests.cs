using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class LanguageLevelTests
{
    private readonly LanguageLevelAttribute _attribute = new();

    public LanguageLevelTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData((int)LanguageLevel.A1)]
    [InlineData((int)LanguageLevel.A2)]
    [InlineData((int)LanguageLevel.B1)]
    [InlineData((int)LanguageLevel.B2)]
    [InlineData((int)LanguageLevel.C1)]
    [InlineData((int)LanguageLevel.C2)]
    [InlineData((int)LanguageLevel.Native)]
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
