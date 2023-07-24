using Microsoft.Extensions.Logging;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Models;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.DataAccess.EF.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TT.Deliveries.DataAccess.EF.Repositories;

/// <summary>
/// Repository that provides methods for Recipient entity
/// </summary>
public class RecipientRepository : BaseCrudRepository<Recipient>, IRecipientRepository
{
    private readonly DeliveriesContext _context;

    public RecipientRepository(DeliveriesContext context, ILogger<Recipient> logger) : base(context, logger)
    {
        _context = context;
    }

    /// <summary>
    /// Get Recipient by userName
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<Recipient> GetByNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _executor.InvokeAsync(async () =>
        {
            return await _context
                .Recipients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserName.Equals(userName), cancellationToken);
        });
    }
}
