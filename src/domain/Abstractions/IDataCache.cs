using System;
using System.Threading.Tasks;

namespace domain.Abstractions
{
    public interface IDataCache
    {
        Task CacheData(string key, object value, TimeSpan expire);
        Task<string?> GetCachedData(string key);
        Task DeleteCache(string key);
        Task DeleteCacheByKeyPattern(string pattern);
    }
}
