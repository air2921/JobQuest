using domain.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datahub.Redis;

public class DataCache(RedisContext context, ILogger<DataCache> logger) : IDataCache
{
    private readonly IDatabase _db = context.GetDatabase();

    public async Task CacheDataAsync(string key, object value, TimeSpan expires)
    {
        try
        {
            var redisValue = await _db.StringGetAsync(key);
            if (redisValue.HasValue)
                await _db.KeyDeleteAsync(key);

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var dataToSave = JsonConvert.SerializeObject(value, settings);
            await _db.StringSetAsync(key, dataToSave, expires);

            // FormatException at this line, idk whi it happens.
            // No one variable int this log not null.
            // Maybe it because of '\n' in log message
            logger.LogInformation($"Request to save data in redis cluster\n" +
                $"Information about saved data:\n" +
                $"Key: {key}\nValue: {dataToSave ?? "NULL"}\nExpires: {expires}");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
        }
    }

    public async Task<string?> GetCacheAsync(string key)
    {
        try
        {
            var value = await _db.StringGetAsync(key);

            // FormatException at this line, idk whi it happens.
            // No one variable int this log not null.
            // Maybe it because of '\n' in log message
            logger.LogInformation($"Request to get data from redis cluster\n" +
                $"Information about the requested data:\n" +
                $"Key: {key}\nValue: {GetStringValue(value)}");

            return value;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
            return default;
        }
    }

    public async Task DeleteCacheAsync(string key)
    {
        try
        {
            var value = await _db.StringGetAsync(key);
            if (value.HasValue)
                await _db.KeyDeleteAsync(key);

            // FormatException at this line, idk whi it happens.
            // No one variable int this log not null.
            // Maybe it because of '\n' in log message
            logger.LogInformation($"Request to delete data by key from redis cluster\n" +
                $"Information about deleted data\n" +
                $"Key: {key}\nValue: {GetStringValue(value)}");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
        }
    }

    public async Task DeleteCacheByKeyPatternAsync(string pattern)
    {
        try
        {
            var redisKeys = await _db.ExecuteAsync("KEYS", "*");
            var result = redisKeys.ToDictionary().Keys;

            if (result == null)
                return;

            logger.LogInformation($"Request to delete data by pattern from redis cluster\nPattern: {pattern}");

            var deletedKeys = new List<string>();
            var partsOfPattern = pattern.Split('_');

            var tasks = result
                .Where(x => x.StartsWith(pattern))
                .ToHashSet()
                .Select(async match =>
                {
                    if (partsOfPattern[0].Equals(match[0]) && partsOfPattern[1].Equals(match[1]))
                    {
                        await _db.KeyDeleteAsync(match);
                        deletedKeys.Add(match);
                    }
                });

            await Task.WhenAll(tasks);

            logger.LogInformation(string.Join(", ", deletedKeys));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());
        }
    }

    private static string GetStringValue(RedisValue? redisValue)
    {
        string? dataToReturn = null;

        if (redisValue.HasValue)
            dataToReturn = redisValue;

        var temp = dataToReturn is not null ? dataToReturn : "Requested value is null";
        return temp;
    }
}
