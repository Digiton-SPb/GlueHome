using TT.Deliveries.Common.Contracts;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Domain.Contracts;

/// <summary>
/// Interface that defines methods for Order repository
/// </summary>
public interface IOrderRepository : IBaseCrudRepository<Order>
{
}