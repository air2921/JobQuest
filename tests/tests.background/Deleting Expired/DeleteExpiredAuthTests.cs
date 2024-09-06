using background;
using Microsoft.Extensions.Logging;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;

namespace tests.background.Deleting_Expired;

public class DeleteExpiredAuthTests
{
    private readonly DeleteExpiredAuth _service;

    private const int MAX = 10000;

    private readonly Mock<ILogger<DeleteExpiredAuth>> _mockLogger;
    private readonly Mock<IRepository<AuthModel>> _mockRepository;

    public DeleteExpiredAuthTests()
    {
        _mockLogger = new Mock<ILogger<DeleteExpiredAuth>>();
        _mockRepository = new Mock<IRepository<AuthModel>>();

        _service = new DeleteExpiredAuth(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Delete_Success_MaxExceeded()
    {
        _mockRepository.Setup(r => r.GetCount(It.Is<CountAuthSpec>(spec => spec.IsExpired == true)))
            .Returns(12000);

        var tokenList = new List<AuthModel>
        {
            new() { TokenId = 1 },
            new() { TokenId = 2 }
        };

        _mockRepository.Setup(r => r.GetRangeAsync(
            It.Is<SortAuthSpec>(spec => spec.Skip == 0 && spec.Count == MAX &&
            spec.OrderByDesc == false && spec.IsExpired == true), null, CancellationToken.None))
            .ReturnsAsync(tokenList);

        await _service.DeleteExpired();

        _mockRepository.Verify(r => r.GetCount(It.Is<CountAuthSpec>(spec => spec.IsExpired == true)), Times.Once);
        _mockRepository.Verify(r => r.GetRangeAsync(It.Is<SortAuthSpec>(spec =>
            spec.Count == MAX && spec.Skip == 0 && spec.IsExpired == true && spec.OrderByDesc == false), null, CancellationToken.None), Times.Once);
        _mockRepository.Verify(r => r.DeleteRangeAsync(It.Is<int[]>(ids =>
            ids.Length == tokenList.Count && ids.Contains(1) && ids.Contains(2)), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Delete_Success_MaxNotExceeded()
    {
        var count = 2;
        var tokenList = new List<AuthModel>
        {
            new() { TokenId = 1 },
            new() { TokenId = 2 }
        };

        _mockRepository.Setup(r => r.GetCount(It.Is<CountAuthSpec>(spec => spec.IsExpired == true)))
            .Returns(count);

        _mockRepository.Setup(r => r.GetRangeAsync(
            It.Is<SortAuthSpec>(spec => spec.Skip == 0 && spec.Count == count &&
            spec.OrderByDesc == false && spec.IsExpired == true), null, CancellationToken.None))
            .ReturnsAsync(tokenList);

        await _service.DeleteExpired();

        _mockRepository.Verify(r => r.GetCount(It.Is<CountAuthSpec>(spec => spec.IsExpired == true)), Times.Once);
        _mockRepository.Verify(r => r.GetRangeAsync(It.Is<SortAuthSpec>(spec =>
            spec.Count == count && spec.Skip == 0 && spec.IsExpired == true && spec.OrderByDesc == false), null, CancellationToken.None), Times.Once);
        _mockRepository.Verify(r => r.DeleteRangeAsync(It.Is<int[]>(ids =>
            ids.Length == tokenList.Count && ids.Contains(1) && ids.Contains(2)), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Delete_ThrowsException()
    {
        _mockRepository.Setup(r => r.GetCount(It.Is<CountAuthSpec>(spec => spec.IsExpired == true)))
            .Throws(new Exception());

        await _service.DeleteExpired();

        _mockRepository.Verify(r => r.GetCount(It.Is<CountAuthSpec>(spec => spec.IsExpired == true)), Times.Once);
        _mockRepository.Verify(r => r.GetRangeAsync(It.Is<SortAuthSpec>(spec =>
            spec.Count == MAX && spec.Skip == 0 && spec.IsExpired == true && spec.OrderByDesc == false), null, CancellationToken.None), Times.Never);
        _mockRepository.Verify(r => r.DeleteRangeAsync(It.IsAny<int[]>(), CancellationToken.None), Times.Never);
    }
}
