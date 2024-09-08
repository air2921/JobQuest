using api.Utils;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace tests.api.Utils;

public class UserInfoTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public UserInfoTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    }

    [Fact]
    public void Constructor_UserNotAuthenticated_ThrowsException()
    {
        var httpContextAccessorMock = Mock.Of<IHttpContextAccessor>();
        Assert.Throws<InvalidOperationException>(() => new UserInfo(httpContextAccessorMock));
    }

    [Fact]
    public void UserId_ClaimTypeNotPresent_ThrowsException()
    {
        var claims = Array.Empty<Claim>();
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        var userData = new UserInfo(_mockHttpContextAccessor.Object);

        Assert.Throws<InvalidOperationException>(() => userData.UserId);
    }

    [Fact]
    public void UserId_ValidClaim_ReturnsCorrectValue()
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, "123")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        var userData = new UserInfo(_mockHttpContextAccessor.Object);

        var result = userData.UserId;

        Assert.Equal(123, result);
    }

    [Theory]
    [InlineData(nameof(UserInfo.Role))]
    public void Property_ClaimTypeNotPresent_ThrowsException(string propertyName)
    {
        var claims = Array.Empty<Claim>();
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        var userData = new UserInfo(_mockHttpContextAccessor.Object);

        switch (propertyName)
        {
            case nameof(UserInfo.Role):
                Assert.Throws<InvalidOperationException>(() => userData.Role);
                break;
            default:
                throw new ArgumentException("Invalid property name", nameof(propertyName));
        }
    }

    [Theory]
    [InlineData(ClaimTypes.Role, "Admin")]
    public void Property_ValidClaim_ReturnsCorrectValue(string claimType, string expectedValue)
    {
        var claims = new[]
        {
                new Claim(claimType, expectedValue)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        var userData = new UserInfo(_mockHttpContextAccessor.Object);

        string? result = null;
        switch (claimType)
        {
            case ClaimTypes.Role:
                result = userData.Role;
                break;
        }

        Assert.Equal(expectedValue, result);
    }
}
