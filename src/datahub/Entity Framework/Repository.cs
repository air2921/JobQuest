using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using domain.Exceptions;

namespace datahub.Entity_Framework;

public class Repository<T> : IRepository<T> where T : class
{
    #region Const

    private const string REQUEST_TIMED_OUT = "Request timed out";
    private const string ERROR = "Unexpected error";
    private const int GET_ALL_AWAITING = 20;
    private const int GET_BY_FILTER_AWAITING = 20;
    private const int GET_BY_ID_AWAITING = 20;
    private const int ADD_AWAITING = 20;
    private const int ADD_RANGE_AWAITING = 20;
    private const int DELETE_AWAITING = 20;
    private const int DELETE_RANGE_AWAITING = 90;
    private const int DELETE_BY_FILTER = 20;
    private const int UPDATE_AWAITING = 20;

    #endregion

    #region fields and constructor

    private readonly ILogger<Repository<T>> _logger;
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ILogger<Repository<T>> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    #endregion

    public int GetCount(ISpecification<T>? specification, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GET_ALL_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            IQueryable<T> query = _dbSet;
            if (specification is not null)
                query = SpecificationEvaluator.Default.GetQuery(query, specification);

            return query.Count();
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException(ERROR);
        }
    }

    public async Task<IEnumerable<T>> GetRangeAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GET_ALL_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            IQueryable<T> query = _dbSet;
            if (specification is not null)
                query = SpecificationEvaluator.Default.GetQuery(query, specification);

            return await query.ToListAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException(ERROR);
        }
    }

    public async Task<T?> GetByFilterAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GET_BY_FILTER_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            IQueryable<T> query = _dbSet;
            query = SpecificationEvaluator.Default.GetQuery(query, specification);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException(ERROR);
        }
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(GET_BY_ID_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException(ERROR);
        }
    }

    public async Task<int> AddAsync(T entity, Func<T, int>? GetId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ADD_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            if (GetId is not null)
                return GetId(entity);

            return 0;
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException("Error when creating entity");
        }
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(ADD_RANGE_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException("Error when creating entity");
        }
    }

    public async Task<T?> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(DELETE_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
            if (entity is not null)
            {
                var deletedEntity = _dbSet.Remove(entity).Entity;
                await _context.SaveChangesAsync(cancellationToken);
                return deletedEntity;
            }
            return null;
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException("Error when deleting data");
        }
    }

    public async Task<IEnumerable<T?>> DeleteRangeAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(DELETE_RANGE_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            var deletedEntities = new List<T>();

            foreach (var id in identifiers)
            {
                var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
                if (entity != null)
                {
                    deletedEntities.Add(entity);
                    _dbSet.Remove(entity);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
            return deletedEntities;
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException("Error when deleting data");
        }
    }

    public async Task<T?> DeleteByFilterAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(DELETE_BY_FILTER));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            IQueryable<T> query = _dbSet;
            query = SpecificationEvaluator.Default.GetQuery(query, specification);

            var entity = await query.FirstOrDefaultAsync(cancellationToken);
            if (entity is not null)
            {
                var deletedEntity = _dbSet.Remove(entity).Entity;
                await _context.SaveChangesAsync(cancellationToken);
                return deletedEntity;
            }
            return null;
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException("Error when deleting data");
        }
    }

    public async Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(UPDATE_AWAITING));
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token).Token;

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }
        catch (OperationCanceledException)
        {
            throw new EntityException(REQUEST_TIMED_OUT);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.ToString(), nameof(_context), nameof(_dbSet));
            throw new EntityException("Error when updating data");
        }
    }
}
