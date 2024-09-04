using application.Components;
using datahub.Redis;
using domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests.application.Components;

public class AttemptValidatorTests
{
    private readonly Mock<IDataCache<ConnectionSecondary>> _mockDataCache;
    private readonly string _srcEmail = "johndoe134@gmail.com";
    private readonly int _acceptableAttempts = 5;
    private readonly int _delay = 15;
    private readonly string _encodedEmail;

    public AttemptValidatorTests()
    {
        _encodedEmail = Convert.ToBase64String(Encoding.UTF32.GetBytes(_srcEmail));
        _mockDataCache = new Mock<IDataCache<ConnectionSecondary>>();
    }

    [Fact]
    public async Task IsValidTry_Valid()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedEmail)).ReturnsAsync(1);
        var service = new AttemptValidator(_mockDataCache.Object);
        var result = await service.IsValidTry(_srcEmail, _acceptableAttempts);

        _mockDataCache.Verify(x => x.GetSingleAsync<int>(_encodedEmail), Times.Once);
        Assert.True(result);
    }

    [Fact]
    public async Task IsValidTry_Invalid()
    {
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedEmail)).ReturnsAsync(6);
        var service = new AttemptValidator(_mockDataCache.Object);
        var result = await service.IsValidTry(_srcEmail, _acceptableAttempts);

        _mockDataCache.Verify(x => x.GetSingleAsync<int>(_encodedEmail), Times.Once);
        Assert.False(result);
    }

    [Fact]
    public async Task AddAttempt_Success()
    {
        var count = 1;
        _mockDataCache.Setup(x => x.GetSingleAsync<int>(_encodedEmail)).ReturnsAsync(count);
        var service = new AttemptValidator(_mockDataCache.Object);
        await service.AddAttempt(_srcEmail, _delay);

        _mockDataCache.Verify(x => x.SetAsync(_encodedEmail, count + 1, TimeSpan.FromMinutes(_delay)));
    }
}
