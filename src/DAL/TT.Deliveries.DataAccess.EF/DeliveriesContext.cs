using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.DataAccess.EF;

/// <summary>
/// Db context for Deliveries database
/// </summary>
public class DeliveriesContext : DbContext
{
    #region Ctor
    public DeliveriesContext(DbContextOptions<DeliveriesContext> options)
        : base(options)
    {
    }
    #endregion

    #region Db Sets
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Recipient> Recipients => Set<Recipient>();
    #endregion

    #region Protected Override Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Delivery>().OwnsOne(p => p.AccessWindow);
    }
    #endregion
}
