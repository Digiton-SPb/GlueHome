using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TT.Deliveries.Common.Contracts;
using TT.Deliveries.Common.Helpers;

namespace TT.Deliveries.DataAccess.EF.Common;

/// <summary>
/// Base repository that provides CRUD methods
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public class BaseCrudRepository<TEntity> : IBaseCrudRepository<TEntity> where TEntity : class, IEntity
{
    #region Protected Fields
    protected readonly ILogger<TEntity> _logger;
    protected readonly Executor _executor;
    protected DbContext _dbContext;
    #endregion

    #region Ctor
    protected BaseCrudRepository(DbContext dbContext, ILogger<TEntity> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _executor = new Executor(_logger);
        _executor.PerformanceLogLevel = LogLevel.Information;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Create method for entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            if (saveChanges)
                await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        });
    }

    /// <summary>
    /// Update method for entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await _executor.InvokeAsync(async () =>
        {
            _dbContext.Update(entity);
            if (saveChanges)
                await _dbContext.SaveChangesAsync(cancellationToken);
        });
    }

    /// <summary>
    /// Delete method for entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await _executor.InvokeAsync(async () =>
        {
            _dbContext.Remove(entity);
            if (saveChanges)
                await _dbContext.SaveChangesAsync(cancellationToken);
        });
    }

    /// <summary>
    /// Save changes in context
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default) 
    {
        await _executor.InvokeAsync(async () =>
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        });
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        {
            return await _dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        });
    }

    /// <summary>
    /// Get entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        { 
            return await _dbContext
                .Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        });
    }
    #endregion
}
