using Microsoft.Extensions.Logging;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Models;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.DataAccess.EF.Common;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using TT.Deliveries.Domain.Dto;

namespace TT.Deliveries.DataAccess.EF.Repositories;

/// <summary>
/// Repository that provides methods for Delivery entity
/// </summary>
public class DeliveryRepository : BaseCrudRepository<Delivery>, IDeliveryRepository
{
    private readonly DeliveriesContext _context;

    public DeliveryRepository(DeliveriesContext context, ILogger<Delivery> logger) : base(context, logger)
    {
        _context = context;
    }

    /// <summary>
    /// Overriden Get Delivery by id, that uncludes Recipient and Order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<IEnumerable<Delivery>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        { 
            return await _dbContext
                .Set<Delivery>()
                .Include("Recipient")
                .Include("Order")
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        });
    }

    /// <summary>
    /// Overriden Get Delivery by id, that uncludes Recipient and Order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<Delivery> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        { 
            return await _dbContext
                .Set<Delivery>()
                .Include("Recipient")
                .Include("Order")
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        });
    }

    /// <summary>
    /// Get Potentially expired Deliveries on date
    /// </summary>
    /// <param name="date"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Delivery>> GetPotentiallyExpiredAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        {
            return await _dbContext
                .Set<Delivery>()
                .Where(p => p.State != DeliveryState.Expired && p.AccessWindow.EndTime < date)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        });
    }
}
