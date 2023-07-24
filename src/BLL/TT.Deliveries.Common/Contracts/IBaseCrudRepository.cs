using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TT.Deliveries.Common.Contracts {

     /// <summary>
     /// Interface of base repository that provides CRUD methods
     /// </summary>
     /// <typeparam name="TEntity"></typeparam>
     public interface IBaseCrudRepository<TEntity>
     {
          /// <summary>
          /// Create entity
          /// </summary>
          /// <param name="entity"></param>
          /// <param name="saveChanges"></param>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          Task<TEntity> CreateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);
          
          /// <summary>
          /// Update entity
          /// </summary>
          /// <param name="entity"></param>
          /// <param name="saveChanges"></param>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          Task UpdateAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);
          
          /// <summary>
          /// Delete entity
          /// </summary>
          /// <param name="entity"></param>
          /// <param name="saveChanges"></param>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          Task DeleteAsync(TEntity entity, bool saveChanges = true, CancellationToken cancellationToken = default);
          
          /// <summary>
          /// Save changes
          /// </summary>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          Task SaveChangesAsync(CancellationToken cancellationToken = default);
          
          /// <summary>
          /// Get all entities
          /// </summary>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
          
          /// <summary>
          /// Get entity by id
          /// </summary>
          /// <param name="id"></param>
          /// <param name="cancellationToken"></param>
          /// <returns></returns>
          Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
     }
}