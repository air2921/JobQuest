using domain.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace application.Helpers
{
    public class CacheDeserialize(IDataCache cacheData, ILogger<CacheDeserialize> logger)
    {
        public async Task<T?> GetDeserializedObject<T>(string key)
        {
            try
            {
                var cache = await cacheData.GetCacheAsync(key);
                if (cache is null)
                    return default;

                return JsonSerializer.Deserialize<T>(cache);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return default;
            }
        }

        public async Task<IEnumerable<T>?> GetDeserializedCollection<T>(string key)
        {
            try
            {
                var cache = await cacheData.GetCacheAsync(key);
                if (cache is null)
                    return null;

                return JsonSerializer.Deserialize<IEnumerable<T>>(cache);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
