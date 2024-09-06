using application.Components;
using application.Utils;
using application.Workflows.Auth;
using common.DTO;
using datahub.Redis;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.User;
using JsonLocalizer;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests.application.Workflows.Auth;

public class LoginTests
{
    private readonly LoginWk _service;

    private readonly string _email = "email";
    private readonly string _password = "password";

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
        var attemptValidatorStub = new AttemptValidatorStub(_mockDataCache.Object);
        var tokenPublisherStub = new TokenPublisherStub(_mockConfiguration.Object, _mockGenerate.Object);

        _service = new LoginWk(_mockUserRepository.Object, _mockAuthRepository.Object, _mockTransaction.Object,
            _mockSender.Object, _mockHashUtility.Object, _mockGenerate.Object, _mockDataCache.Object,
            attemptValidatorStub, tokenPublisherStub, _mockHostEnvironment.Object, _mockLocalizer.Object);
        _mockTransaction.Setup(x => x.Begin()).Returns(_mockDbTransaction.Object);
    }

    [Fact]
    public async Task Initiate_UserNotFound()
    {
        _mockUserRepository.Setup(x => x.GetByFilterAsync
        (It.Is<UserByEmailSpec>(x => x.Email == _email), null, CancellationToken.None)).ReturnsAsync((UserModel)null);

        var result = await _service.Initiate(new LoginDTO { Email = _email, Password = _password });
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockSender.Verify(x => x.SendMessage(It.IsAny<EmailDTO>(), CancellationToken.None), Times.Never);
        _mockDataCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never);
    }


    private class AttemptValidatorStub(IDataCache<ConnectionSecondary> dataCache) : AttemptValidator(dataCache)
    {

    }

    public class TokenPublisherStub(IConfiguration configuration, IGenerate generate) : TokenPublisher(configuration, generate)
    {

    }
}
