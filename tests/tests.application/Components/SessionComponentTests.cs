using application.Components;
using application.Utils;
using common.DTO;
using domain.Abstractions;
using domain.Enums;
using domain.Includes;
using domain.Models;
using domain.Specifications.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace tests.application.Components;

public class SessionComponentTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IGenerate> _mockGenerate;

    private readonly Mock<IRepository<AuthModel>> _mockRepository;
    private readonly UserModel _userModel;
    private readonly string _refresh = "refresh_token";
    private readonly string _jwt = "jwt_token";

    public SessionComponentTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockGenerate = new Mock<IGenerate>();

        _mockRepository = new Mock<IRepository<AuthModel>>();
        _userModel = new UserModel { UserId = 2921, Role = Role.Admin.ToString() };
    }

    [Fact]
    public async Task RefreshJsonWebToken_Success()
    {
        _mockRepository.Setup(x => x.GetByFilterAsync(It.Is<AuthByValueSpec>(x => x.Value == _refresh),
            It.Is<AuthInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync(new AuthModel { Value = _refresh, UserId = 2921, User = _userModel });

        var service = new SessionComponent(_mockRepository.Object,
            new TokenPublisherStub(_mockConfiguration.Object, _mockGenerate.Object, _userModel, _jwt));
        var result = await service.RefreshJsonWebToken(_refresh);

        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.Equal(_jwt, ObjectValueGetter.GetData(result.ObjectData!, "jwt"));
    }

    [Fact]
    public async Task RefreshJsonWebToken_TokenNotFound()
    {
        _mockRepository.Setup(x => x.GetByFilterAsync(It.Is<AuthByValueSpec>(x => x.Value == _refresh),
            It.Is<AuthInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync((AuthModel)null);

        var service = new SessionComponent(_mockRepository.Object, null);
        var result = await service.RefreshJsonWebToken(_refresh);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);
    }

    [Fact]
    public async Task RefreshJsonWebToken_UserNotFound()
    {
        _mockRepository.Setup(x => x.GetByFilterAsync(It.Is<AuthByValueSpec>(x => x.Value == _refresh),
            It.Is<AuthInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync(new AuthModel { Value = _refresh, UserId = 2921, User = null });

        var service = new SessionComponent(_mockRepository.Object, null);
        var result = await service.RefreshJsonWebToken(_refresh);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);
    }

    private class TokenPublisherStub(IConfiguration configuration, IGenerate generate, UserModel userModel, string jwt) 
        : TokenPublisher(configuration, generate)
    {
        public override string JsonWebToken(JwtDTO dto)
        {
            if (dto.Role != userModel.Role || dto.UserId != userModel.UserId || dto.Expires != Immutable.JwtExpires)
                return $"{dto.Role}:{userModel.Role}-{dto.UserId}:{userModel.UserId}{dto.Expires}:{Immutable.JwtExpires}";

            return jwt;
        }

        public override string RefreshToken()
        {
            return base.RefreshToken();
        }
    }
}
