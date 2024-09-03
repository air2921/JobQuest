using infrastructure.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests.infrasturcture.Utils;

public class HashUtilityTests
{
    private readonly HashUtility _hashUtility;
    private readonly Mock<ILogger<HashUtility>> _loggerMock;

    public HashUtilityTests()
    {
        _loggerMock = new Mock<ILogger<HashUtility>>();
        _hashUtility = new HashUtility(_loggerMock.Object);
    }

    [Fact]
    public void Hash_Success()
    {
        var password = "password123";
        var hash = _hashUtility.Hash(password);

        Assert.NotEmpty(hash);
        Assert.NotEqual(password, hash);
    }

    [Fact]
    public void Verify_ForValidPassword()
    {
        var password = "password123";
        var hash = _hashUtility.Hash(password);
        var result = _hashUtility.Verify(password, hash);

        Assert.True(result);
    }

    [Fact]
    public void Verify_ForInvalidPassword()
    {
        var correctPassword = "correctPassword";
        var incorrectPassword = "incorrectPassword";
        var hash = _hashUtility.Hash(correctPassword);
        var result = _hashUtility.Verify(incorrectPassword, hash);

        Assert.False(result);
    }
}
