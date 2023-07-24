using System;

namespace TT.Deliveries.Domain.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string Sender { get; set; }
    }
}