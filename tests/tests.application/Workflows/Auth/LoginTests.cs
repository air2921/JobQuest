using application.Components;
using application.Utils;
using application.Workflows.Auth;
using Ardalis.Specification;
using common.DTO;
using common.Exceptions;
using datahub.Redis;
using domain.Abstractions;
using domain.Enums;
using domain.Localize;
using domain.Models;
using domain.Specifications.User;
using JsonLocalizer;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text;
using static application.Workflows.Auth.LoginWk;

namespace tests.application.Workflows.Auth;

public class LoginTests
{
    private readonly LoginWk _service;

    private readonly string _email = "email";
    private readonly string _password = "password";
    private readonly string _passwordHash = "password_hash";
    private readonly string _token = "token";
    private readonly int _code = 12345678;
    private readonly string _user = "user";
    private readonly string _emailBody = "body";
    private readonly string _emailHead = "head";
    private readonly string _json_token = "json_token";
    private readonly string _refresh_token = "refresh_token";
    private readonly int _userId = 2921;
    private readonly LoginDTO _loginDTO;
    private readonly string _encoded_email;
    private readonly string _encodedToken;

    private readonly Mock<IConfiguration> _mockConfiguration = new();
    private readonly Mock<IDbContextTransaction> _mockDbTransaction = new();

    private readonly Mock<IGenerate> _mockGenerate = new();
    private readonly Mock<IRepository<UserModel>> _mockUserRepository = new();
    private readonly Mock<IRepository<AuthModel>> _mockAuthRepository = new();
    private readonly Mock<IDatabaseTransaction> _mockTransaction = new();
    private readonly Mock<ISender<EmailDTO>> _mockSender = new();
    private readonly Mock<IHashUtility> _mockHashUtility = new();
    private readonly Mock<IDataCache<ConnectionSecondary>> _mockDataCache = new();
    private readonly Mock<IHostEnvironment> _mockHostEnvironment = new();
    private readonly Mock<ILocalizer> _mockLocalizer = new();

    public LoginTests()
    {
        _encoded_email = Convert.ToBase64String(Encoding.UTF32.GetBytes(_email));
        _encodedToken = Convert.ToBase64String(Encoding.UTF32.GetBytes(_token));
        _loginDTO = new LoginDTO { Email = _email, Password = _password };

        var attemptValidator = new AttemptValidator(_mockDataCache.Object);
        var tokenPublisherStub = new TokenPublisherStub(_mockConfiguration.Object, _mockGenerate.Object, _json_token, _refresh_token);

        _service = new LoginWk(_mockUserRepository.Object, _mockAuthRepository.Object, _mockTransaction.Object,
            _mockSender.Object, _mockHashUtility.Object, _mockGenerate.Object, _mockDataCache.Object,
            attemptValidator, tokenPublisherStub, _mockHostEnvironment.Object, _mockLocalizer.Object);
        _mockTransaction.Setup(x => x.Begin()).Returns(_mockDbTransaction.Object);
    }

    [Fact]
    public async Task Initiate_InvalidAttempt()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encoded_email)).ReturnsAsync(11);

        var result = await _service.Initiate(_loginDTO);
        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockUserRepository.Verify(x => x.GetByFilterAsync
        (It.IsAny<ISpecification<UserModel>>(), null, CancellationToken.None), Times.Never);
        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_UserNotFound()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None)).ReturnsAsync((UserModel)null);

        var result = await _service.Initiate(_loginDTO);
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_UserBlocked()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(new UserModel { IsBlocked = true });

        var result = await _service.Initiate(_loginDTO);
        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_IncorrectPassword()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(new UserModel { IsBlocked = false, PasswordHash = _passwordHash });

        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encoded_email)).ReturnsAsync(1);
        _mockHashUtility.Setup(x => x.Verify(_password, _passwordHash)).Returns(false);

        var result = await _service.Initiate(_loginDTO);
        Assert.False(result.IsSuccess);
        Assert.Equal(401, result.Status);
        Assert.Null(result.ObjectData);

        _mockDataCache.Verify(x => x.SetAsync(_encoded_email, 2, TimeSpan.FromMinutes(15)), Times.Once);
        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockDataCache.Verify(x => x.SetAsync(_token, It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_SmtpThrowsException()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(new UserModel { IsBlocked = false, PasswordHash = _passwordHash });

        _mockLocalizer.Setup(x => x.Translate(Names.USER, null)).Returns(_user);
        _mockLocalizer.Setup(x => x.Translate(Mail.LOGIN_CONFIRM_BODY, null)).Returns(_emailBody);
        _mockLocalizer.Setup(x => x.Translate(Mail.LOGIN_CONFIRM_HEAD, null)).Returns(_emailHead);

        _mockHashUtility.Setup(x => x.Verify(_password, _passwordHash)).Returns(true);
        _mockGenerate.Setup(x => x.GenerateCode(8)).Returns(_code);
        _mockSender.Setup(x => x.SendMessage(It.Is<EmailDTO>(x => x.Email == _email && x.Username == _user &&
        x.Body == _emailBody + _code && x.Subject == _emailHead),
            CancellationToken.None))
            .ThrowsAsync(new SmtpClientException(It.IsAny<string>()));

        var result = await _service.Initiate(_loginDTO);
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_RepositoryThrowsException()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ThrowsAsync(new EntityException(It.IsAny<string>()));

        var result = await _service.Initiate(_loginDTO);
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_Success_Env_Prod()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(new UserModel { IsBlocked = false, PasswordHash = _passwordHash,
            UserId = 2921, Role = Role.Employer.ToString() });

        _mockLocalizer.Setup(x => x.Translate(Names.USER, null)).Returns(_user);
        _mockLocalizer.Setup(x => x.Translate(Mail.LOGIN_CONFIRM_BODY, null)).Returns(_emailBody);
        _mockLocalizer.Setup(x => x.Translate(Mail.LOGIN_CONFIRM_HEAD, null)).Returns(_emailHead);

        _mockHashUtility.Setup(x => x.Verify(_password, _passwordHash)).Returns(true);
        _mockGenerate.Setup(x => x.GenerateCode(8)).Returns(_code);
        _mockGenerate.Setup(x => x.GuidCombine(3, true)).Returns(_token);

        var result = await _service.Initiate(_loginDTO);
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.ObjectData);
        Assert.Null(ObjectValueGetter.GetData(result.ObjectData, "code"));
        Assert.Equal(_token, ObjectValueGetter.GetData(result.ObjectData, "uniqueToken"));

        _mockSender.Verify(x => x.SendMessage(It.Is<EmailDTO>(x => x.Email == _email && x.Username == _user &&
            x.Body == _emailBody + _code && x.Subject == _emailHead),
            CancellationToken.None));
        _mockDataCache.Verify(x => x.SetAsync(_token,
            It.Is<UserObject>(x => x.UserId == 2921 && x.Code == _code && x.Role == Role.Employer.ToString()),
            TimeSpan.FromMinutes(10)), Times.Once);
    }

    [Fact]
    public async Task Complete_InvalidAttempt()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(11);

        var result = await _service.Complete(_code, _token);
        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockDataCache.Verify(x => x.GetSingleAsync<UserObject>(It.IsAny<string>()), Times.Never);
        _mockDataCache.Verify(x => x.DeleteSingleAsync(It.IsAny<string>()), Times.Never);
        _mockAuthRepository.Verify(x => x.AddAsync(It.IsAny<AuthModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Complete_UserObjectNotFound()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockDataCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync((UserObject)null);

        var result = await _service.Complete(_code, _token);
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockDataCache.Verify(x => x.DeleteSingleAsync(It.IsAny<string>()), Times.Never);
        _mockAuthRepository.Verify(x => x.AddAsync(It.IsAny<AuthModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Complete_IncorrectCode()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockDataCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(new UserObject(123, 1, ""));

        var result = await _service.Complete(_code, _token);
        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockDataCache.Verify(x => x.SetAsync(_encodedToken, 2, TimeSpan.FromMinutes(15)), Times.Once);
        _mockDataCache.Verify(x => x.DeleteSingleAsync(It.IsAny<string>()), Times.Never);
        _mockAuthRepository.Verify(x => x.AddAsync(It.IsAny<AuthModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Complete_CannotDeleteUserObjectFromCache()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockDataCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(new UserObject(_code, _userId, ""));
        _mockDataCache.Setup(x => x.DeleteSingleAsync(_token)).ReturnsAsync(false);

        var result = await _service.Complete(_code, _token);
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbTransaction.Verify(x => x.Rollback(), Times.Once);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockAuthRepository.Verify(x => x.AddAsync(
            It.Is<AuthModel>(x => x.UserId == _userId && x.Value == _refresh_token),
            null, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Complete_RepositoryThrowsException()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockDataCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(new UserObject(_code, _userId, ""));
        _mockAuthRepository.Setup(x => x.AddAsync(It.IsAny<AuthModel>(), null, CancellationToken.None))
            .ThrowsAsync(new EntityException(""));

        var result = await _service.Complete(_code, _token);
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbTransaction.Verify(x => x.Rollback(), Times.Once);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockDataCache.Verify(x => x.DeleteSingleAsync(_token), Times.Never);
    }

    [Fact]
    public async Task Complete_Success()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockDataCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(new UserObject(_code, _userId, ""));
        _mockDataCache.Setup(x => x.DeleteSingleAsync(_token)).ReturnsAsync(true);

        var result = await _service.Complete(_code, _token);
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.ObjectData);
        Assert.Equal(_refresh_token, ObjectValueGetter.GetData(result.ObjectData, "refresh"));
        Assert.Equal(_json_token, ObjectValueGetter.GetData(result.ObjectData, "auth"));

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbTransaction.Verify(x => x.Commit(), Times.Once);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockAuthRepository.Verify(x => x.AddAsync(
            It.Is<AuthModel>(x => x.UserId == _userId && x.Value == _refresh_token),
            null, CancellationToken.None), Times.Once);
    }

    public class TokenPublisherStub(IConfiguration configuration, IGenerate generate, string jwt, string refresh)
        : TokenPublisher(configuration, generate)
    {
        public override string JsonWebToken(JwtDTO dto)
        {
            return jwt;
        }

        public override string RefreshToken()
        {
            return refresh;
        }
    }
}
