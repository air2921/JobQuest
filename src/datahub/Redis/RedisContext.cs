using common;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Linq;

namespace datahub.Redis;

public class RedisContext
{
    private readonly IConfiguration _config;
    private readonly ConnectionMultiplexer _multiplexer;

    public RedisContext(IConfiguration config)
    {
        _config = config;
        var connectionStr = _config.GetConnectionString(App.REDIS_DB) ?? throw new InvalidOperationException();
        _multiplexer = ConnectionMultiplexer.Connect(connectionStr);
    }

    public IDatabase GetDatabase() => _multiplexer.GetDatabase();
    public IServer GetServer() => _multiplexer.GetServer(_multiplexer.GetEndPoints().First());
}
