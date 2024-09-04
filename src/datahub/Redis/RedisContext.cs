using common;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace datahub.Redis;

public class RedisContext : IRedisContext
{
    private readonly IConfiguration _config;
    private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> _multiplexers = [];

    public RedisContext(IConfiguration config)
    {
        _config = config;

        var redisDatabases = _config.GetSection(App.REDIS_SECTION).GetChildren();

        foreach (var db in redisDatabases)
        {
            var name = db.GetValue<string>(App.REDIS_NAME);
            var connectionStr = db.GetValue<string>(App.REDIS_CONNECTION);

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(connectionStr))
                throw new InvalidOperationException("Invalid Redis configuration");

            _multiplexers.TryAdd(name, ConnectionMultiplexer.Connect(connectionStr));
        }
    }

    public IDatabase GetDatabase(string dbName) =>
        _multiplexers.TryGetValue(dbName, out var multiplexer)
            ? multiplexer.GetDatabase()
            : throw new ArgumentException($"Database with name {dbName} does not exist");

    public IServer GetServer(string dbName) =>
        _multiplexers.TryGetValue(dbName, out var multiplexer)
            ? multiplexer.GetServer(multiplexer.GetEndPoints().First())
            : throw new ArgumentException($"Database with name {dbName} does not exist");

    public static void Dispose()
    {
        foreach (var multiplexer in _multiplexers.Values)
            multiplexer.Dispose();
    }
}

public interface IRedisContext
{
    IDatabase GetDatabase(string dbName);
    IServer GetServer(string dbName);
}
