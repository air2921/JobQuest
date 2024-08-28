using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IDataCache<T> where T : IConnection
{
    Task SetAsync(string key, object value, TimeSpan expires);
    Task<TObject?> GetSingleAsync<TObject>(string key);
    Task<IEnumerable<TObject>?> GetRangeAsync<TObject>(string key);
    Task DeleteSingleAsync(string key);
    Task DeleteRangeAsync(IEnumerable<string> keys);
    Task DeleteRangeByPatternAsync(string pattern);
}

public interface IConnection
{
    public string ConnectionName { get; init; }
}
