using domain.Abstractions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace application.Helpers
{
    public class CacheDeserialize<T>(IDataCache cacheData)
    {
        public async Task<T?> GetDeserialized(string key)
        {
            var cache = await cacheData.GetCacheAsync(key);
            if (cache is null)
                return default;

            return JsonConvert.DeserializeObject<T>(cache);
        }
    }
}
