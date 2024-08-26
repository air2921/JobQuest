using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IDataCache
{
    void Change(string name);
    Task SetAsync(string key, object value, TimeSpan expires);
    Task<T?> GetSingleAsync<T>(string key);
    Task<IEnumerable<T>?> GetRangeAsync<T>(string key);
    Task DeleteSingleAsync(string key);
    Task DeleteRangeAsync(IEnumerable<string> keys);
    Task DeleteRangeByPatternAsync(string pattern);
}
