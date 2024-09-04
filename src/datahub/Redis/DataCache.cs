using common;
using domain.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datahub.Redis;

public class ConnectionPrimary : IConnection
{
    public string ConnectionName { get; init; } = App.REDIS_PRIMARY;
}

public class ConnectionSecondary : IConnection
{
    public string ConnectionName { get; init; } = App.REDIS_SECONDARY;
}

public class RedisChatConnection : IConnection
{
    public string ConnectionName { get; init; } = App.REDIS_CHAT;
}

public class DataCache<T> : IDataCache<T> where T : IConnection
{
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
    private readonly IRedisContext _context;
    private readonly ILogger<DataCache<T>> _logger;
    private readonly IServer _server;
    private readonly IDatabase _db;

    public DataCache(IRedisContext context, ILogger<DataCache<T>> logger, T connection)
    {
        _context = context;
        _logger = logger;
        _db = _context.GetDatabase(connection.ConnectionName);
        _server = _context.GetServer(connection.ConnectionName);
    }

    public async Task<bool> SetAsync(string key, object value, TimeSpan expires)
    {
        try
        {
            var redisValue = await _db.StringGetAsync(key);
            if (redisValue.HasValue)
                await _db.KeyDeleteAsync(key);

            var dataToSave = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
            await _db.StringSetAsync(key, dataToSave, expires);

            _logger.LogInformation($"Request to save data in redis\nKey: {key}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, key, value.ToString(), expires.TotalSeconds);
            return false;
        }
    }

    public async Task<TObject?> GetSingleAsync<TObject>(string key)
    {
        try
        {
            var value = await _db.StringGetAsync(key);

            _logger.LogInformation($"Request to get data from redis\nKey: {key}");

            if (!value.HasValue)
                return default;

            var jsonValue = value.ToString();
            return JsonConvert.DeserializeObject<TObject>(jsonValue);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, key);
            return default;
        }
    }

    public async Task<IEnumerable<TObject>?> GetRangeAsync<TObject>(string key)
    {
        try
        {
            var value = await _db.StringGetAsync(key);
            if (!value.HasValue)
                return null;

            _logger.LogInformation($"Request to get data from redis\nKey: {key}");

            return JsonConvert.DeserializeObject<IEnumerable<TObject>>(value!);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, key);
            return null;
        }
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<string> keys)
    {
        try
        {
            var tasks = new List<Task>();

            foreach (var key in keys)
                tasks.Add(_db.KeyDeleteAsync(key));
            await Task.WhenAll(tasks);

            var allKeys = LogRange(keys);
            _logger.LogInformation($"Request to delete range data by keys from redis\nKeys: {allKeys}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteSingleAsync(string key)
    {
        try
        {
            await _db.KeyDeleteAsync(key);

            _logger.LogInformation($"Request to delete data by key from redis\nKey: {key}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, key);
            return false;
        }
    }

    public async Task<bool> DeleteRangeByPatternAsync(RedisValue pattern)
    {
        try
        {
            var keys = _server.Keys(pattern: pattern);

            if (!keys.Any())
                return true;

            var tasks = keys
                .Select(key => _db.KeyDeleteAsync(key))
                .ToList();

            await Task.WhenAll(tasks);

            _logger.LogInformation($"Request to delete range data by key pattern\n Pattern: {pattern}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, pattern);
            return false;
        }
    }

    private static string LogRange(IEnumerable<string> keys)
    {
        var builder = new StringBuilder(keys.Sum(x => x.Length));
        foreach (var key in keys)
            builder.AppendLine(key);

        return builder.ToString();
    }
}
