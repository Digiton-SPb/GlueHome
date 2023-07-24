using System.Threading;
using System.Threading.Tasks;
using TT.Deliveries.Common.Contracts;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Domain.Contracts;

/// <summary>
/// Interface that defines methods for Recipient repository
/// </summary>
public interface IRecipientRepository : IBaseCrudRepository<Recipient>
{
    Task<Recipient> GetByNameAsync(string userName, CancellationToken cancellationToken = default);
}