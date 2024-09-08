using application.Workflows.Auth;
using Ardalis.Specification;
using common.Exceptions;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using JsonLocalizer;
using Microsoft.EntityFrameworkCore.Storage;

namespace tests.application.Workflows.Auth;

public class LogoutTests
{
    private readonly LogoutWk _service;

    private readonly Mock<IRepository<AuthModel>> _mockRepository;
    private readonly Mock<IDatabaseTransaction> _mockTransaction;
    private readonly Mock<IDbContextTransaction> _mockContextTransaction;
    private readonly Mock<ILocalizer> _mockLocalizer;

    private readonly string _refresh = "token";
    private readonly int _userId = 2921;

    public LogoutTests()
    {
        _mockRepository = new Mock<IRepository<AuthModel>>();
        _mockContextTransaction = new Mock<IDbContextTransaction>();
        _mockTransaction = new Mock<IDatabaseTransaction>();
        _mockLocalizer = new Mock<ILocalizer>();
        _mockTransaction.Setup(x => x.Begin()).Returns(_mockContextTransaction.Object);

        _service = new LogoutWk(_mockRepository.Object, _mockTransaction.Object, _mockLocalizer.Object);
    }

    [Fact]
    public async Task Logout_AuthNotFound()
    {
        _mockRepository.Setup(x => x.DeleteByFilterAsync(It.Is<AuthByValueSpec>(x => x.Value == _refresh),
            CancellationToken.None)).ReturnsAsync((AuthModel)null);

        var result = await _service.Logout(_refresh, false);

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Status);
        Assert.Null(result.ObjectData);

        _mockRepository.Verify(x => x.GetCount(It.IsAny<ISpecification<AuthModel>>()), Times.Never);
        _mockRepository.Verify(x => x.DeleteRangeAsync(It.IsAny<IEnumerable<int>>(), CancellationToken.None), Times.Never);
        _mockRepository.Verify(x => x.GetRangeAsync(It.IsAny<ISpecification<AuthModel>>(),
            null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Logout_RepositoryThrowsException()
    {
        _mockRepository.Setup(x => x.DeleteByFilterAsync(It.Is<AuthByValueSpec>(x => x.Value == _refresh),
            CancellationToken.None)).ReturnsAsync(new AuthModel { UserId = _userId });
        _mockRepository.Setup(x => x.GetCount(It.IsAny<ISpecification<AuthModel>>()))
            .Throws(new EntityException(""));

        var result = await _service.Logout(_refresh, true);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockContextTransaction.Verify(x => x.Rollback(), Times.Once);
        _mockRepository.Verify(x => x.DeleteRangeAsync(It.IsAny<IEnumerable<int>>(), CancellationToken.None), Times.Never);
        _mockRepository.Verify(x => x.GetRangeAsync(It.IsAny<ISpecification<AuthModel>>(),
            null, CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task Logout_Success()
    {
        var count = 10;

        _mockRepository.Setup(x => x.DeleteByFilterAsync(It.Is<AuthByValueSpec>(x => x.Value == _refresh),
            CancellationToken.None)).ReturnsAsync(new AuthModel { UserId = _userId });
        _mockRepository.Setup(x => x.GetCount(It.Is<CountAuthSpec>(x => x.UserId == _userId)))
            .Returns(count);
        _mockRepository.Setup(x => x.GetRangeAsync(
            It.Is<SortAuthSpec>(x => x.SkipCount == 0 && x.Count == count && x.OrderByDesc == true && x.UserId == _userId),
            null, CancellationToken.None)).ReturnsAsync([new AuthModel { TokenId = 1 }, new AuthModel { TokenId = 2 }]);

        var result = await _service.Logout(_refresh, true);

        Assert.True(result.IsSuccess);
        Assert.Equal(204, result.Status);
        Assert.Null(result.ObjectData);

        _mockTransaction.Verify(x => x.Begin(), Times.Once);
        _mockContextTransaction.Verify(x => x.Commit(), Times.Once);
        _mockRepository.Verify(x => x.DeleteRangeAsync(It.Is<IEnumerable<int>>(x => x.Contains(1) && x.Contains(2)), CancellationToken.None), Times.Once);
    }
}
