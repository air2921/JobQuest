using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.Specification;
using System.Linq.Expressions;

namespace domain.Abstractions;

public interface IRepository<T> where T : class
{
    int GetCount(ISpecification<T>? specification);
    Task<IEnumerable<T>> GetRangeAsync(ISpecification<T>? specification = null, IInclude<T>? include = null, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByFilterAsync(ISpecification<T> specification, IInclude<T>? include = null, CancellationToken cancellationToken = default);
    Task<T?> GetByIdWithInclude(IEntityById<T> specification, IInclude<T>? include = null, CancellationToken cancellationToken = default);
    Task<int> AddAsync(T entity, Func<T, int>? GetId = null, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> DeleteByFilterAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken = default);
}
