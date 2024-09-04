using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IDataCache<T> where T : IConnection
{
    Task<bool> SetAsync(string key, object value, TimeSpan expires);
    Task<TObject?> GetSingleAsync<TObject>(string key);
    Task<IEnumerable<TObject>?> GetRangeAsync<TObject>(string key);
    Task<bool> DeleteSingleAsync(string key);
    Task<bool> DeleteRangeAsync(IEnumerable<string> keys);
    Task<bool> DeleteRangeByPatternAsync(RedisValue pattern);
}

public interface IConnection
{
    public string ConnectionName { get; init; }
}
