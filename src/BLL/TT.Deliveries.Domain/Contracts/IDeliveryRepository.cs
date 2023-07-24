using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TT.Deliveries.Common.Contracts;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Domain.Contracts;

/// <summary>
/// Interface that defines methods for Repository repository
/// </summary>
public interface IDeliveryRepository : IBaseCrudRepository<Delivery>
{
    /// <summary>
    /// Get potentially expired Deliveries with AccessWindow.EndTime in past, related to provided date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Delivery>> GetPotentiallyExpiredAsync(DateTime date, CancellationToken cancellationToken = default);
}
