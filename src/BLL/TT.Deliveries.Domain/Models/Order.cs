using System;
using TT.Deliveries.Common.Contracts;

namespace TT.Deliveries.Domain.Models;

/// <summary>
/// Order entity
/// </summary>
public class Order : IEntity
{
    public Order() {}
    public Order(string orderNumber, string sender)
    {
        this.OrderNumber = orderNumber;
        this.Sender = sender;
    }

    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string Sender { get; set; }
}