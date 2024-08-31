using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IGenericCache<T> where T : class
{
    Task<IEnumerable<T>?> GetRangeAsync(string key, Func<Task<IEnumerable<T>>> dataReceiverCallback, int expirationMin = 10);
    Task<T?> GetSingleAsync(string key, Func<Task<T?>> dataReceiverCallback, int expirationMin = 10);
}
