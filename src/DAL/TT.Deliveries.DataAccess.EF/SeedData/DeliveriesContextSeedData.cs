using System;
using System.Linq;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.DataAccess.EF.SeedData;

/// <summary>
/// Contains methods for initial seed of data
/// </summary>
public static class DeliveriesContextSeedData
{
    #region Public Static Methods
    /// <summary>
    /// Seed initial data
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = (DeliveriesContext)serviceProvider.GetService(typeof(DeliveriesContext));
        if (context == null) return;

        if (!context.Deliveries.Any())
            SeedDeliveries(context);
    }
    #endregion

    #region Private Methods
    private static void SeedDeliveries(DeliveriesContext context)
    {
        var recipient = GetRecipient();
        var now = DateTime.Now;
        
        for(int i = 0; i < 3; i++)
        {
            var delivery = new Delivery
            {
                AccessWindow = new AccessWindow(now.AddDays(-2+i), now.AddDays(-1+i)),
                Recipient = recipient,
                Order = GetOrder(i)
            };
            context.Deliveries.Add(delivery);
        }
        context.SaveChanges();
    }

    private static Recipient GetRecipient()
    {
        return new Recipient { 
            UserName = "dmitry", 
            Name = "Dmitry", 
            Address = "Long Road",
        };
    }

    private static Order GetOrder(int num)
    {
        return new Order { 
            OrderNumber = $"O-100{num}", 
            Sender = "Super Store"
        };
    }
    #endregion
}
