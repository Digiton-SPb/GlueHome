namespace TT.Deliveries.Domain.Dto
{
    public enum DeliveryState
    {
        /// <summary>
        /// Defines a newly created delivery
        /// </summary>
        Created = 0,

        /// <summary>
        /// The delivery has been approved by the recipient
        /// </summary>
        Approved = 1,

        /// <summary>
        /// The delivery has been successfully completed
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The delivery has been cancelled
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// The delivery has expired - outside permitted access window
        /// </summary>
        Expired = 4
    }
}