using domain.Abstractions;
using domain.Models;
using datahub.Redis;
using common.DTO;
using JsonLocalizer;
using Microsoft.EntityFrameworkCore.Storage;
using application.Workflows.Auth;
using application.Components;
using domain.Specifications.User;
using common.Exceptions;
using domain.Localize;
using static application.Workflows.Auth.RegisterWk;
using domain.Enums;
using System.Text;

namespace tests.application.Workflows.Auth;

public class RegistrationTests
{
    private readonly RegisterWk _service;

    private readonly Mock<IRepository<UserModel>> _mockUserRepository;
    private readonly Mock<IDatabaseTransaction> _mockTransaction;
    private readonly Mock<IDbContextTransaction> _mockDbContextTransaction;
    private readonly Mock<IDataCache<ConnectionSecondary>> _mockCache;
    private readonly Mock<ISender<EmailDTO>> _mockSender;
    private readonly Mock<IGenerate> _mockGenerate;
    private readonly Mock<IHashUtility> _mockHashUtility;
    private readonly Mock<ILocalizer> _mockLocalizer;

    private readonly string _email = "email";
    private readonly string _role = Role.Employer.ToString();
    private readonly string _password = "password";
    private readonly string _passwordHash = "hash";
    private readonly string _firstName = "f_name";
    private readonly string _lastName = "l_name";
    private readonly string? _patronymic = null;
    private readonly string _emailBody = "e_body";
    private readonly string _emailHead = "e_head";
    private readonly int _code = 12345678;
    private readonly string _token = "token";
    private readonly string _encodedToken;
    private readonly RegisterDTO _registerDTO;
    private readonly UserObject _userObject;

    public RegistrationTests()
    {
        _mockUserRepository = new Mock<IRepository<UserModel>>();
        _mockCache = new Mock<IDataCache<ConnectionSecondary>>();
        _mockSender = new Mock<ISender<EmailDTO>>();
        _mockGenerate = new Mock<IGenerate>();
        _mockHashUtility = new Mock<IHashUtility>();
        _mockLocalizer = new Mock<ILocalizer>();

        _mockTransaction = new Mock<IDatabaseTransaction>();
        _mockDbContextTransaction = new Mock<IDbContextTransaction>();
        _mockTransaction.Setup(x => x.Begin()).Returns(_mockDbContextTransaction.Object);
        _encodedToken = Convert.ToBase64String(Encoding.UTF32.GetBytes(_token));

        _service = new RegisterWk(_mockUserRepository.Object, _mockTransaction.Object, _mockCache.Object,
            new AttemptValidator(_mockCache.Object), _mockSender.Object, _mockGenerate.Object,
            _mockHashUtility.Object, _mockLocalizer.Object);

        _registerDTO = new RegisterDTO
        {
            Email = _email,
            Password = _password,
            AsEmployer = true,
            Patronymic = _patronymic,
            FirstName = _firstName,
            LastName = _lastName
        };

        _userObject = new UserObject(_email, _passwordHash, _role, _code, _firstName, _lastName, _patronymic);
    }

    [Fact]
    public async Task Initiate_UserExists()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(new UserModel());

        var result = await _service.Initiate(_registerDTO);

        Assert.False(result.IsSuccess);
        Assert.Equal(409, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Initiate_RepositoryThrowException()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ThrowsAsync(new EntityException(""));

        var result = await _service.Initiate(_registerDTO);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Initiate_SmtpThrowsException()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
                .ReturnsAsync((UserModel)null);

        _mockGenerate.Setup(x => x.GenerateCode(8)).Returns(_code);
        _mockLocalizer.Setup(x => x.Translate(Mail.REGISTRATION_CONFIRM_BODY, null)).Returns(_emailBody);
        _mockLocalizer.Setup(x => x.Translate(Mail.REGISTRATION_CONFIRM_HEAD, null)).Returns(_emailHead);

        _mockSender.Setup(x => x.SendMessage(It.Is<EmailDTO>(x => x.Username == It.IsAny<string>() && x.Email == _email &&
            x.Body == _emailBody + _code && x.Subject == _emailHead),
            CancellationToken.None)).ThrowsAsync(new SmtpClientException(""));

        var result = await _service.Initiate(_registerDTO);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Initiate_Success()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
                .ReturnsAsync((UserModel)null);

        _mockGenerate.Setup(x => x.GenerateCode(8)).Returns(_code);
        _mockGenerate.Setup(x => x.GuidCombine(3, true)).Returns(_token);
        _mockHashUtility.Setup(x => x.Hash(_password)).Returns(_passwordHash);
        _mockLocalizer.Setup(x => x.Translate(Mail.REGISTRATION_CONFIRM_BODY, null)).Returns(_emailBody);
        _mockLocalizer.Setup(x => x.Translate(Mail.REGISTRATION_CONFIRM_HEAD, null)).Returns(_emailHead);

        var result = await _service.Initiate(_registerDTO);

        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.NotNull(result.ObjectData);
        Assert.Equal(_token, ObjectValueGetter.GetData(result.ObjectData, "uniqueToken"));

        _mockSender.Verify(x => x.SendMessage(It.Is<EmailDTO>(x => x.Username == It.IsAny<string>() && x.Email == _email &&
            x.Body == _emailBody + _code && x.Subject == _emailHead),
            CancellationToken.None), Times.Once);
        _mockCache.Verify(x => x.SetAsync(_token, It.Is<UserObject>(x => x.Role == _role && x.Code == _code &&
        x.Email == _email && x.FirstName == _firstName && x.LastName == _lastName && x.Patronymic == _patronymic &&
        x.Password == _passwordHash),
            TimeSpan.FromMinutes(10)), Times.Once);
    }

    [Fact]
    public async Task Complete_InvalidAttempt()
    {
        _mockCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(11);

        var result = await _service.Complete(_code, _token);

        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.GetSingleAsync<UserObject>(_token), Times.Never);
        _mockCache.Verify(x => x.SetAsync(_encodedToken, It.IsAny<int>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<UserModel>(), null, CancellationToken.None), Times.Never);
        _mockCache.Verify(x => x.DeleteSingleAsync(_token), Times.Never);
    }

    [Fact]
    public async Task Complete_UserObjectNotFound()
    {
        _mockCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync((UserObject)null);

        var result = await _service.Complete(_code, _token);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.SetAsync(_encodedToken, It.IsAny<int>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<UserModel>(), null, CancellationToken.None), Times.Never);
        _mockCache.Verify(x => x.DeleteSingleAsync(_token), Times.Never);
    }

    [Fact]
    public async Task Complete_IncorrectCode()
    {
        _mockCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(_userObject);

        var result = await _service.Complete(123, _token);

        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.SetAsync(_encodedToken, 2, TimeSpan.FromMinutes(15)), Times.Once);
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<UserModel>(), null, CancellationToken.None), Times.Never);
        _mockCache.Verify(x => x.DeleteSingleAsync(_token), Times.Never);
    }

    [Fact]
    public async Task Complete_RepositoryThrowsException()
    {
        _mockCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(_userObject);
        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<UserModel>(), null, CancellationToken.None))
            .ThrowsAsync(new EntityException(""));

        var result = await _service.Complete(_code, _token);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockCache.Verify(x => x.SetAsync(_encodedToken, It.IsAny<int>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockCache.Verify(x => x.DeleteSingleAsync(_token), Times.Never);
    }

    [Fact]
    public async Task Complete_UserObjectNotDeleted()
    {
        _mockCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(_userObject);
        _mockCache.Setup(x => x.DeleteSingleAsync(_token)).ReturnsAsync(false);

        var result = await _service.Complete(_code, _token);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbContextTransaction.Verify(x => x.Rollback(), Times.Once);
        _mockCache.Verify(x => x.SetAsync(_encodedToken, It.IsAny<int>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockUserRepository.Verify(x => x.AddAsync(It.Is<UserModel>(x => x.IsBlocked == false && x.Email == _email &&
        x.FirstName == _firstName && x.LastName == _lastName && x.PasswordHash == _passwordHash && x.Patronymic == _patronymic &&
        x.Role == _role), null, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Complete_Success()
    {
        _mockCache.Setup(x => x.GetSingleAsync<int>(_encodedToken)).ReturnsAsync(1);
        _mockCache.Setup(x => x.GetSingleAsync<UserObject>(_token)).ReturnsAsync(_userObject);
        _mockCache.Setup(x => x.DeleteSingleAsync(_token)).ReturnsAsync(true);

        var result = await _service.Complete(_code, _token);

        Assert.True(result.IsSuccess);
        Assert.Equal(201, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbContextTransaction.Verify(x => x.Commit(), Times.Once);
        _mockCache.Verify(x => x.SetAsync(_encodedToken, It.IsAny<int>(), It.IsAny<TimeSpan>()), Times.Never);
        _mockUserRepository.Verify(x => x.AddAsync(It.Is<UserModel>(x => x.IsBlocked == false && x.Email == _email &&
        x.FirstName == _firstName && x.LastName == _lastName && x.PasswordHash == _passwordHash && x.Patronymic == _patronymic &&
        x.Role == _role), null, CancellationToken.None), Times.Once);
    }
}
