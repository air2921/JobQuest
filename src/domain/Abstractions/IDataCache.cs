﻿using System;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IDataCache
{
    void Change(string name);
    Task CacheDataAsync(string key, object value, TimeSpan expires);
    Task<string?> GetCacheAsync(string key);
    Task DeleteCacheAsync(string key);
    Task DeleteCacheByPatternAsync(string pattern);
}
