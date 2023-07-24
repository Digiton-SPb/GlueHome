namespace TT.Deliveries.Domain.Dto
{
    public class CreateDeliveryDto
    {
        public AccessWindowDto AccessWindow { get; set; }

        public RecipientDto Recipient { get; set; }

        public OrderDto Order { get; set; }        
    }
}
