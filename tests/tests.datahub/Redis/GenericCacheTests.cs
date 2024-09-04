using datahub.Redis;
using domain.Abstractions;

namespace tests.datahub.Redis;

public class GenericCacheTests
{
    private readonly int _expires = 10;
    private readonly string _key = "key";
    private readonly string _value = "value";
    private readonly string[] _valueCollection = ["value1", "value2", "value3"];
    private readonly Mock<Func<Task<string?>>> _mockObjectCallBack;
    private readonly Mock<Func<Task<IEnumerable<string>>>> _mockCollectionCallBack;

    private readonly Mock<IDataCache<ConnectionPrimary>> _mockCache;

    public GenericCacheTests()
    {
        _mockCache = new Mock<IDataCache<ConnectionPrimary>>();
        _mockObjectCallBack = new Mock<Func<Task<string?>>>();
        _mockCollectionCallBack = new Mock<Func<Task<IEnumerable<string>>>>();
    }

    [Fact]
    public async Task GetRange_CacheIsNotNull()
    {
        _mockCache.Setup(x => x.GetRangeAsync<string>(_key)).ReturnsAsync(_valueCollection);

        var genericCache = new GenericCache<string>(_mockCache.Object);
        var result = await genericCache.GetRangeAsync(_key, _mockCollectionCallBack.Object, _expires);

        _mockCollectionCallBack.Verify(cb => cb(), Times.Never);
        _mockCache.Verify(x => x.GetRangeAsync<string>(_key), Times.Once);
        Assert.Equal(_valueCollection, result);
    }

    [Fact]
    public async Task GetRange_CacheIsNull_CallbackReturnsCollection()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockCache.Setup(x => x.GetRangeAsync<string>(_key)).ReturnsAsync((IEnumerable<string>)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockCollectionCallBack.Setup(cb => cb()).ReturnsAsync(_valueCollection);

        var genericCache = new GenericCache<string>(_mockCache.Object);
        var result = await genericCache.GetRangeAsync(_key, _mockCollectionCallBack.Object, _expires);

        _mockCollectionCallBack.Verify(cb => cb(), Times.Once);
        _mockCache.Verify(x => x.GetRangeAsync<string>(_key), Times.Once);
        _mockCache.Verify(x => x.SetAsync(_key, _valueCollection, TimeSpan.FromMinutes(_expires)));
        Assert.Equal(_valueCollection, result);
    }

    [Fact]
    public async Task GetSingle_CacheIsNotNull()
    {
        _mockCache.Setup(x => x.GetSingleAsync<string>(_key)).ReturnsAsync(_value);

        var genericCache = new GenericCache<string>(_mockCache.Object);
        var result = await genericCache.GetSingleAsync(_key, _mockObjectCallBack.Object!, _expires);

        _mockCache.Verify(x => x.GetSingleAsync<string>(_key), Times.Once);
        _mockObjectCallBack.Verify(cb => cb(), Times.Never);
        Assert.Equal(_value, result);
    }

    [Fact]
    public async Task GetSingle_CacheIsNull_CallbackReturnsNull()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockCache.Setup(x => x.GetSingleAsync<string>(_key)).ReturnsAsync((string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockObjectCallBack.Setup(cb => cb()).ReturnsAsync((string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        var genericCache = new GenericCache<string>(_mockCache.Object);
        var result = await genericCache.GetSingleAsync(_key, _mockObjectCallBack.Object!, _expires);

        _mockCache.Verify(x => x.GetSingleAsync<string>(_key), Times.Once);
        _mockObjectCallBack.Verify(cb => cb(), Times.Once);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSingle_CacheIsNull_CallbackReturnsNotNull()
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockCache.Setup(x => x.GetSingleAsync<string>(_key)).ReturnsAsync((string)null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        _mockObjectCallBack.Setup(cb => cb()).ReturnsAsync(_value);

        var genericCache = new GenericCache<string>(_mockCache.Object);
        var result = await genericCache.GetSingleAsync(_key, _mockObjectCallBack.Object!, _expires);

        _mockCache.Verify(x => x.GetSingleAsync<string>(_key), Times.Once);
        _mockObjectCallBack.Verify(cb => cb(), Times.Once);
        Assert.Equal(_value, result);
    }
}