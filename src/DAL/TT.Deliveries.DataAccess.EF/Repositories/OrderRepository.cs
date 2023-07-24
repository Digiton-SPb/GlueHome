using Microsoft.Extensions.Logging;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Models;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.DataAccess.EF.Common;

namespace TT.Deliveries.DataAccess.EF.Repositories;

/// <summary>
/// Repository that provides methods for Order entity
/// </summary>
public class OrderRepository : BaseCrudRepository<Order>, IOrderRepository
{
    private readonly DeliveriesContext _context;

    public OrderRepository(DeliveriesContext context, ILogger<Order> logger) : base(context, logger)
    {
        _context = context;
    }
}
