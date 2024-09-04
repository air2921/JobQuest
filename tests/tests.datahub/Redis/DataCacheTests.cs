using datahub.Redis;
using domain.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace tests.datahub.Redis;

public class DataCacheTests
{
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

    private readonly Mock<ILogger<DataCache<IConnection>>> _mockLogger;
    private readonly Mock<IRedisContext> _mockContext;
    private readonly Mock<IConnection> _mockConnection;
    private readonly Mock<IServer> _mockServer;
    private readonly Mock<IDatabase> _mockDatabase;

    private readonly RedisValue _pattern = "key";
    private readonly RedisKey[] _redisKeys = ["key1", "key2", "key3"];
    private readonly RedisKey[] _redisKeysEmpty = [];
    private readonly string _key = "testKey";
    private readonly string[] _keys = ["key1", "key2", "key3"];
    private readonly string _stringValueToCache = "Some value i want to cache";
    private readonly string[] _arrayToCache = ["value1", "value2", "value3"];
    private readonly int _intValueToCache = 2921;
    private readonly string _getValue = "value";
    private readonly TimeSpan _expires = TimeSpan.FromSeconds(30);

    public DataCacheTests()
    {
        _mockLogger = new Mock<ILogger<DataCache<IConnection>>>();
        _mockConnection = new Mock<IConnection>();
        _mockServer = new Mock<IServer>();
        _mockDatabase = new Mock<IDatabase>();

        _mockContext = new Mock<IRedisContext>();
        _mockContext.Setup(x => x.GetServer(It.IsAny<string>())).Returns(_mockServer.Object);
        _mockContext.Setup(x => x.GetDatabase(It.IsAny<string>())).Returns(_mockDatabase.Object);
    }

    [Fact]
    public async Task Set_HasPreviousValue_Success()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(_getValue);

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.SetAsync(_key, _stringValueToCache, _expires);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(_key, CommandFlags.None), Times.Once);

        var json = JsonConvert.SerializeObject(_stringValueToCache, _jsonSerializerSettings);
        _mockDatabase.Verify(x => x.StringSetAsync(_key, json, _expires, false, When.Always, CommandFlags.None), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task Set_HasntPreviousValue_Success()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.SetAsync(_key, _stringValueToCache, _expires);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(_key, CommandFlags.None), Times.Never);

        var json = JsonConvert.SerializeObject(_stringValueToCache, _jsonSerializerSettings);
        _mockDatabase.Verify(x => x.StringSetAsync(_key, json, _expires, false, When.Always, CommandFlags.None), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task GetSingle_ReferenceType_Success()
    {
        var json = JsonConvert.SerializeObject(_stringValueToCache, _jsonSerializerSettings);
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(json);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetSingleAsync<string>(_key);

        Assert.Equal(_stringValueToCache, result);
    }

    [Fact]
    public async Task GetSingle_ValueType_Success()
    {
        var json = JsonConvert.SerializeObject(_intValueToCache, _jsonSerializerSettings);
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(json);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetSingleAsync<int>(_key);

        Assert.Equal(_intValueToCache, result);
    }

    [Fact]
    public async Task GetRange_Success()
    {
        var json = JsonConvert.SerializeObject(_arrayToCache, _jsonSerializerSettings);
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(json);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetRangeAsync<string>(_key);

        Assert.NotNull(result);
        Assert.Equal(_arrayToCache.Length, result.Count());
    }

    [Fact]
    public async Task DeleteRange_Success()
    {
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteRangeAsync(_keys);

        foreach (var key in _keys)
            _mockDatabase.Verify(x => x.KeyDeleteAsync(key, CommandFlags.None), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteSingle_Success()
    {
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteSingleAsync(_key);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(_key, CommandFlags.None), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRangeByPattern_Success()
    {
        _mockServer.Setup(x => x.Keys(-1, _pattern, 250, 0, 0, CommandFlags.None)).Returns(_redisKeys);

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteRangeByPatternAsync(_pattern);

        foreach (var key in _redisKeys)
            _mockDatabase.Verify(x => x.KeyDeleteAsync(key, CommandFlags.None), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRangeByPattern_KeysEmpty_Success()
    {
        _mockServer.Setup(x => x.Keys(-1, _pattern, 250, 0, 0, CommandFlags.None)).Returns(_redisKeysEmpty);

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteRangeByPatternAsync(_pattern);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None), Times.Never);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteRangeByPattern_ThrowsException()
    {
        _mockServer.Setup(x => x.Keys(-1, _pattern, 250, 0, 0, CommandFlags.None))
            .Throws(new Exception());

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteRangeByPatternAsync(_pattern);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None), Times.Never);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteRange_ThrowsException()
    {
        _mockDatabase.Setup(x => x.KeyDeleteAsync(_keys.FirstOrDefault(), CommandFlags.None))
            .ThrowsAsync(new Exception());

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteRangeAsync(_keys);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteSingle_ThrowsException()
    {
        _mockDatabase.Setup(x => x.KeyDeleteAsync(_key, CommandFlags.None))
            .ThrowsAsync(new Exception());

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.DeleteSingleAsync(_key);

        Assert.False(result);
    }

    [Fact]
    public async Task GetRange_ValueNotFound()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetRangeAsync<string>(_key);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetRange_ThrowsException()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None))
            .ThrowsAsync(new Exception());
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetRangeAsync<string>(_key);

        Assert.Null(result);
    }


    [Fact]
    public async Task Set_ThrowsException()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None))
            .ThrowsAsync(new Exception());

        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.SetAsync(_key, _stringValueToCache, _expires);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(_key, CommandFlags.None), Times.Never);

        var json = JsonConvert.SerializeObject(_stringValueToCache, _jsonSerializerSettings);
        _mockDatabase.Verify(x => x.StringSetAsync(_key, json, _expires, false, When.Always, CommandFlags.None), Times.Never);

        Assert.False(result);
    }

    [Fact]
    public async Task GetSingle_ValueIsNull_ReferenceType_Fail()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetSingleAsync<string>(_key);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSingle_ValueIsNull_ValueType_Fail()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetSingleAsync<int>(_key);

        Assert.Equal(default, result);
    }

    [Fact]
    public async Task GetSingle_ReferenceType_ThrowsException()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetSingleAsync<string>(_key);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetSingle_ValueType_ThrowsException()
    {
        _mockDatabase.Setup(x => x.StringGetAsync(_key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);
        var dataCache = new DataCache<IConnection>(_mockContext.Object, _mockLogger.Object, _mockConnection.Object);
        var result = await dataCache.GetSingleAsync<int>(_key);

        Assert.Equal(default, result);
    }
}
