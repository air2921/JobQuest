using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace datahub.Redis
{
    public class RedisContext
    {
        private readonly IDatabase _database;
        private readonly IConfiguration _config;

        public RedisContext(IConfiguration config)
        {
            _config = config;
            var connectionStr = _config.GetConnectionString("Redis") ?? throw new InvalidOperationException();
            var connection = ConnectionMultiplexer.Connect(connectionStr);
            _database = connection.GetDatabase();
        }

        public IDatabase GetDatabase() => _database;
    }
}
