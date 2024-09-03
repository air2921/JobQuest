using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class ReasonTests
{
    private readonly ReasonAttribute _attributeNullable = new(false);
    private readonly ReasonAttribute _attributeNonNullable = new(true);

    public ReasonTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData((int)Reason.SalaryLevelInsufficient)]
    [InlineData((int)Reason.InsufficientQualifications)]
    [InlineData((int)Reason.Unreliability)]
    [InlineData((int)Reason.Dishonesty)]
    [InlineData((int)Reason.PoorReferences)]
    [InlineData((int)Reason.FrequentJobChanges)]
    [InlineData((int)Reason.NotMeetingVacancyRequirements)]
    [InlineData((int)Reason.CulturalMisfit)]
    [InlineData((int)Reason.LackOfRelevantExperience)]
    [InlineData((int)Reason.LackOfInterestOrMotivation)]
    [InlineData((int)Reason.VacancyClosed)]
    [InlineData((int)Reason.Other)]
    public void IsValid_NonNullable_Success(int reason)
    {
        var result = _attributeNonNullable.IsValid(reason);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_NonNullable_Fail()
    {
        int reason = 999;
        var result = _attributeNonNullable.IsValid(reason);
        Assert.False(result);
    }

    [Fact]
    public void IsValid_Nullable_Success()
    {
        int? reason = null;
        var result = _attributeNullable.IsValid(reason);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Nullable_Fail()
    {
        int reason = 999;
        var result = _attributeNullable.IsValid(reason);
        Assert.False(result);
    }
}
