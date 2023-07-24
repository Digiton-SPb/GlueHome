using System;
using TT.Deliveries.Common.Contracts;
using TT.Deliveries.Domain.Dto;

namespace TT.Deliveries.Domain.Models;

/// <summary>
/// Delivery entity
/// </summary>
public class Delivery : IEntity
{
    public Guid Id { get; set; }

    public DeliveryState State { get; set; }

    public AccessWindow AccessWindow { get; set; }

    public Guid RecipientId { get; set; }

    public Recipient Recipient { get; set; }

    public Guid OrderId { get; set; }

    public Order Order { get; set; }
}
