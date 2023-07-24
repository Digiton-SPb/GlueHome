using System;

namespace TT.Deliveries.Domain.Dto
{
    public class UpdateDeliveryDto
    {
        public Guid Id { get; set; }

        public AccessWindowDto AccessWindow { get; set; }

        public Guid? RecipientId { get; set; }

        public Guid? OrderId { get; set; }
    }
}
