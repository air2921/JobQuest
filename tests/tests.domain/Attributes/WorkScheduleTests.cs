using domain.Attributes;
using domain.Enums;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class WorkScheduleTests
{
    private readonly WorkScheduleAttribute _attribute = new();

    public WorkScheduleTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData((int)WorkSchedule.FullDay)]
    [InlineData((int)WorkSchedule.Remote)]
    [InlineData((int)WorkSchedule.Flexible)]
    public void IsValid_Success(int schedule)
    {
        var result = _attribute.IsValid(schedule);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Fail()
    {
        var schedule = 999;
        var result = _attribute.IsValid(schedule);
        Assert.False(result);
    }
}
