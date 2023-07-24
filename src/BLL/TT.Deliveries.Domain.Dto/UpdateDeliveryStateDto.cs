using System;

namespace TT.Deliveries.Domain.Dto
{
    public class UpdateDeliveryStateDto
    {
        public Guid Id { get; set; }
        
        public DeliveryState State { get; set; }
    }
}
