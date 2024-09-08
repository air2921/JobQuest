using application.Workflows.Auth;
using common.DTO;
using common.Exceptions;
using domain.Abstractions;
using domain.Enums;
using domain.Includes;
using domain.Localize;
using domain.Models;
using domain.Specifications.Recovery;
using domain.Specifications.User;
using JsonLocalizer;
using Microsoft.EntityFrameworkCore.Storage;

namespace tests.application.Workflows.Auth;

public class RecoveryTests
{
    private readonly RecoveryWk _service;

    private readonly Mock<IRepository<RecoveryModel>> _mockRecoveryRepository;
    private readonly Mock<IRepository<UserModel>> _mockUserRepository;
    private readonly Mock<IDatabaseTransaction> _mockTransaction;
    private readonly Mock<IDbContextTransaction> _mockDbContextTransaction;
    private readonly Mock<ISender<EmailDTO>> _mockSender;
    private readonly Mock<IGenerate> _mockGenerate;
    private readonly Mock<IHashUtility> _mockHashUtility;
    private readonly Mock<ILocalizer> _mockLocalizer;

    private readonly UserModel _userModel;
    private readonly int _userId = 2921;
    private readonly string _role = Role.Employer.ToString();
    private readonly string _firstName = "f_name";
    private readonly string _lastName = "l_name";
    private readonly string? _patronymic = null;
    private readonly string _emailBody = "body";
    private readonly string _emailHead = "head";
    private readonly string _email = "email";
    private readonly string _password = "password";
    private readonly string _newPassword = "new_password";
    private readonly string _newPasswordHash = "new_password_hash";
    private readonly string _token = "token";

    public RecoveryTests()
    {
        _mockRecoveryRepository = new Mock<IRepository<RecoveryModel>>();
        _mockUserRepository = new Mock<IRepository<UserModel>>();
        _mockSender = new Mock<ISender<EmailDTO>>();
        _mockGenerate = new Mock<IGenerate>();
        _mockHashUtility = new Mock<IHashUtility>();
        _mockLocalizer = new Mock<ILocalizer>();

        _mockTransaction = new Mock<IDatabaseTransaction>();
        _mockDbContextTransaction = new Mock<IDbContextTransaction>();
        _mockTransaction.Setup(x => x.Begin()).Returns(_mockDbContextTransaction.Object);

        _service = new RecoveryWk(_mockRecoveryRepository.Object, _mockUserRepository.Object, _mockTransaction.Object,
            _mockSender.Object, _mockHashUtility.Object, _mockGenerate.Object, _mockLocalizer.Object);

        _userModel = new UserModel
        {
            UserId = _userId,
            FirstName = _firstName,
            LastName = _lastName,
            Patronymic = _patronymic,
            Email = _email,
            PasswordHash = _password,
            Role = _role,
            IsBlocked = false,
        };
    }

    [Fact]
    public async Task Initiate_UserNotFound()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync((UserModel)null);

        var result = await _service.Initiate(_email);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockRecoveryRepository.Verify(x => x.AddAsync(It.IsAny<RecoveryModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Initiate_UserBlocked()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(new UserModel { IsBlocked = true });

        var result = await _service.Initiate(_email);

        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockRecoveryRepository.Verify(x => x.AddAsync(It.IsAny<RecoveryModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Initiate_SmtpThrowsException()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(_userModel);

        _mockLocalizer.Setup(x => x.Translate(Mail.ACCOUNT_RECOVERY_BODY, null)).Returns(_emailBody);
        _mockLocalizer.Setup(x => x.Translate(Mail.ACCOUNT_RECOVERY_HEAD, null)).Returns(_emailHead);
        _mockGenerate.Setup(x => x.GuidCombine(3, true)).Returns(_token);

        _mockSender.Setup(x => x.SendMessage(It.Is<EmailDTO>(x => x.Username == It.IsAny<string>() &&
        x.Subject == _emailHead && x.Body == _emailBody + _token && x.Email == _email),
            CancellationToken.None)).ThrowsAsync(new SmtpClientException(""));

        var result = await _service.Initiate(_email);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockRecoveryRepository.Verify(x => x.AddAsync(It.IsAny<RecoveryModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Initiate_RepositoryThrowsException()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ThrowsAsync(new EntityException(""));

        var result = await _service.Initiate(_email);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockRecoveryRepository.Verify(x => x.AddAsync(It.IsAny<RecoveryModel>(), null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Initiate_Success()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync(It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None))
            .ReturnsAsync(_userModel);

        _mockLocalizer.Setup(x => x.Translate(Mail.ACCOUNT_RECOVERY_BODY, null)).Returns(_emailBody);
        _mockLocalizer.Setup(x => x.Translate(Mail.ACCOUNT_RECOVERY_HEAD, null)).Returns(_emailHead);
        _mockGenerate.Setup(x => x.GuidCombine(3, true)).Returns(_token);

        var result = await _service.Initiate(_email);

        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.Is<EmailDTO>(x => x.Username == It.IsAny<string>() &&
            x.Subject == _emailHead && x.Body == _emailBody + _token && x.Email == _email),
            CancellationToken.None));

        _mockRecoveryRepository.Verify(x => x.AddAsync(It.Is<RecoveryModel>(x => x.Value == _token && x.UserId == _userId),
            null, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Complete_UserNotFound()
    {
        _mockRecoveryRepository.Setup(x => x.GetByFilterAsync(It.Is<RecoveryByValueSpec>(x => x.Value == _token),
            It.Is<RecoveryInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync((RecoveryModel)null);

        var result = await _service.Complete(_token, _newPassword);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<UserModel>(), CancellationToken.None), Times.Never);
        _mockRecoveryRepository.Verify(x => x.UpdateAsync(It.IsAny<RecoveryModel>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Complete_UserIsBlocked()
    {
        _mockRecoveryRepository.Setup(x => x.GetByFilterAsync(It.Is<RecoveryByValueSpec>(x => x.Value == _token),
            It.Is<RecoveryInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync(new RecoveryModel { User = new UserModel { IsBlocked = true } });

        var result = await _service.Complete(_token, _newPassword);

        Assert.False(result.IsSuccess);
        Assert.Equal(403, result.Status);
        Assert.Null(result.ObjectData);

        _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<UserModel>(), CancellationToken.None), Times.Never);
        _mockRecoveryRepository.Verify(x => x.UpdateAsync(It.IsAny<RecoveryModel>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Complete_RepositoryThrowsException()
    {
        _mockRecoveryRepository.Setup(x => x.GetByFilterAsync(It.Is<RecoveryByValueSpec>(x => x.Value == _token),
            It.Is<RecoveryInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync(new RecoveryModel { User = new UserModel { IsBlocked = false } });

        _mockRecoveryRepository.Setup(x => x.UpdateAsync(It.IsAny<RecoveryModel>(), CancellationToken.None))
            .ThrowsAsync(new EntityException(""));

        var result = await _service.Complete(_token, _newPassword);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbContextTransaction.Verify(x => x.Rollback(), Times.Once);
        _mockUserRepository.Verify(x => x.UpdateAsync(It.IsAny<UserModel>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Complete_Success()
    {
        _mockRecoveryRepository.Setup(x => x.GetByFilterAsync(It.Is<RecoveryByValueSpec>(x => x.Value == _token),
            It.Is<RecoveryInclude>(x => x.IncludeUser == true), CancellationToken.None))
            .ReturnsAsync(new RecoveryModel { UserId = _userId, User = new UserModel { UserId = _userId, IsBlocked = false } });
        _mockHashUtility.Setup(x => x.Hash(_newPassword)).Returns(_newPasswordHash);

        var result = await _service.Complete(_token, _newPassword);

        Assert.True(result.IsSuccess);
        Assert.Equal(204, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockDbContextTransaction.Verify(x => x.Commit(), Times.Once);

        _mockUserRepository.Verify(x => x.UpdateAsync(It.Is<UserModel>(x => x.PasswordHash == _newPasswordHash && x.UserId == _userId),
            CancellationToken.None), Times.Once);
        _mockRecoveryRepository.Verify(x => x.UpdateAsync(It.Is<RecoveryModel>(x => x.IsUsed == true && x.UserId == _userId),
            CancellationToken.None), Times.Once);
    }
}
