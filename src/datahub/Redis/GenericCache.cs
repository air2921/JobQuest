using domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace datahub.Redis;

public class GenericCache<T>(IDataCache<ConnectionPrimary> dataCache) : IGenericCache<T> where T : class
{
    public async Task<IEnumerable<T>?> GetRangeAsync(string key, Func<Task<IEnumerable<T>>> dataRetrievalCallback, int expirationMin = 10)
    {
        var cache = await dataCache.GetRangeAsync<T>(key);
        if (cache is not null)
            return cache;

        var dbData = await dataRetrievalCallback();
        if (dbData is null)
            return null;

        await dataCache.SetAsync(key, dbData, TimeSpan.FromMinutes(expirationMin));
        return dbData;
    }

    public async Task<T?> GetSingleAsync(string key, Func<Task<T?>> dataRetrievalCallback, int expirationMin = 10)
    {
        var cache = await dataCache.GetSingleAsync<T>(key);
        if (cache is not null)
            return cache;

        var dbData = await dataRetrievalCallback();
        if (dbData is null)
            return null;

        await dataCache.SetAsync(key, dbData, TimeSpan.FromMinutes(expirationMin));
        return dbData;
    }
}
