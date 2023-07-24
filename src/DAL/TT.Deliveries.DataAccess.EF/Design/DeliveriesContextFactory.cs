using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TT.Deliveries.DataAccess.EF.Design;

public class DeliveriesContextFactory : IDesignTimeDbContextFactory<DeliveriesContext>
{
    public DeliveriesContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DeliveriesContext>();
        optionsBuilder.UseSqlite("Data Source=deliveries.db");

        return new DeliveriesContext(optionsBuilder.Options);
    }
}
