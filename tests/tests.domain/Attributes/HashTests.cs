using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class HashTests
{
    private readonly HashAttribute _attribute = new();

    public HashTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Fact]
    public void IsValid_Success()
    {
        var hash = "$2a$11$NbXvhbJEkKlsF2axlu3/B.WrlF.fm2vvD.1Bq7Qx3yGotVM28UGKe";
        var result = _attribute.IsValid(hash);
        Assert.True(result);
    }

    [Fact]
    public void IsValid_Fail()
    {
        var hash = "Some none hashed password";
        var result = _attribute.IsValid(hash);
        Assert.False(result);
    }
}
