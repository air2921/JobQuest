using background;
using domain.Abstractions;
using domain.Models;
using domain.Specifications.Auth;
using domain.Specifications.Recovery;
using Microsoft.Extensions.Logging;

namespace tests.background.Deleting_Expired;

public class DeleteExpiredRecoveryTests
{
    private readonly DeleteExpiredRecovery _service;

    private const int MAX = 10000;

    private readonly Mock<ILogger<DeleteExpiredRecovery>> _mockLogger;
    private readonly Mock<IRepository<RecoveryModel>> _mockRepository;

    public DeleteExpiredRecoveryTests()
    {
        _mockLogger = new Mock<ILogger<DeleteExpiredRecovery>>();
        _mockRepository = new Mock<IRepository<RecoveryModel>>();

        _service = new DeleteExpiredRecovery(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Delete_Success_MaxExceeded()
    {
        _mockRepository.Setup(r => r.GetCount(It.Is<CountRecoverySpec>(spec => spec.IsExpired == true && spec.IsUsed == true)))
            .Returns(12000);

        var tokenList = new List<RecoveryModel>
        {
            new() { TokenId = 1 },
            new() { TokenId = 2 }
        };

        _mockRepository.Setup(r => r.GetRangeAsync(
            It.Is<SortRecoverySpec>(spec => spec.Skip == 0 && spec.Count == MAX &&
            spec.OrderByDesc == false && spec.IsExpired == true), null, CancellationToken.None))
            .ReturnsAsync(tokenList);

        await _service.DeleteExpired();

        _mockRepository.Verify(r => r.GetCount(It.Is<CountRecoverySpec>(spec => spec.IsExpired == true && spec.IsUsed == true)), Times.Once);
        _mockRepository.Verify(r => r.GetRangeAsync(It.Is<SortRecoverySpec>(spec =>
            spec.Count == MAX && spec.Skip == 0 && spec.IsExpired == true
            && spec.OrderByDesc == false && spec.IsUsed == true), null, CancellationToken.None), Times.Once);
        _mockRepository.Verify(r => r.DeleteRangeAsync(It.Is<int[]>(ids =>
            ids.Length == tokenList.Count && ids.Contains(1) && ids.Contains(2)), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Delete_Success_MaxNotExceeded()
    {
        var count = 2;
        var tokenList = new List<RecoveryModel>
        {
            new() { TokenId = 1 },
            new() { TokenId = 2 }
        };

        _mockRepository.Setup(r => r.GetCount(It.Is<CountRecoverySpec>(spec => spec.IsExpired == true && spec.IsUsed == true)))
            .Returns(count);

        _mockRepository.Setup(r => r.GetRangeAsync(
            It.Is<SortRecoverySpec>(spec => spec.Skip == 0 && spec.Count == count &&
            spec.OrderByDesc == false && spec.IsExpired == true && spec.IsUsed == true), null, CancellationToken.None))
            .ReturnsAsync(tokenList);

        await _service.DeleteExpired();

        _mockRepository.Verify(r => r.GetCount(It.Is<CountRecoverySpec>(spec => spec.IsExpired == true && spec.IsUsed == true)), Times.Once);
        _mockRepository.Verify(r => r.GetRangeAsync(It.Is<SortRecoverySpec>(spec =>
            spec.Count == count && spec.Skip == 0 && spec.IsExpired == true &&
            spec.OrderByDesc == false && spec.IsUsed == true), null, CancellationToken.None), Times.Once);
        _mockRepository.Verify(r => r.DeleteRangeAsync(It.Is<int[]>(ids =>
            ids.Length == tokenList.Count && ids.Contains(1) && ids.Contains(2)), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Delete_ThrowsException()
    {
        _mockRepository.Setup(r => r.GetCount(It.Is<CountRecoverySpec>(spec => spec.IsExpired == true && spec.IsUsed == true)))
            .Throws(new Exception());

        await _service.DeleteExpired();

        _mockRepository.Verify(r => r.GetCount(It.Is<CountRecoverySpec>(spec => spec.IsExpired == true && spec.IsUsed == true)), Times.Once);
        _mockRepository.Verify(r => r.GetRangeAsync(It.Is<SortRecoverySpec>(spec =>
            spec.Count == MAX && spec.Skip == 0 && spec.IsExpired == true &&
            spec.OrderByDesc == false && spec.IsUsed == true), null, CancellationToken.None), Times.Never);
        _mockRepository.Verify(r => r.DeleteRangeAsync(It.IsAny<int[]>(), CancellationToken.None), Times.Never);
    }
}
