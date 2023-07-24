using System.Threading.Tasks;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Dto;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Web.Api.Services;

/// <summary>
/// Service that handles changing delivery state by Recipient or Partner with checking allowed state transitions
/// </summary>
public class DeliveriesStateService
{
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliveriesStateService(IDeliveryRepository deliveryRepository) 
    {
        _deliveryRepository = deliveryRepository;
    }

    /// <summary>
    /// Update state of Delivery by Recipient.
    /// Allowed: Created => Approved, Created|Approved => Cancelled
    /// </summary>
    /// <param name="delivery"></param>
    /// <param name="recipient"></param>
    /// <returns></returns>
    public async Task<(ServiceResult, string)> UpdateStateByRecipient(Delivery delivery, Recipient recipient)
    {
        var allowedConditions = 0;
        var existingDelivery = await _deliveryRepository.GetByIdAsync(delivery.Id);
        if (existingDelivery == null)
            return (ServiceResult.NotFound, null);

        if (!existingDelivery.Recipient.Id.Equals(recipient.Id)) 
            return (ServiceResult.Unauthorized, "User not authorized to update this delivery");

        // Check if not same state
        if (delivery.State == existingDelivery.State) 
            return (ServiceResult.Bad, "Recipient can't update State of Delivery to the same value");

        // Check for state transition "Created => Approved"
        if (delivery.State == DeliveryState.Approved && existingDelivery.State == DeliveryState.Created)
            allowedConditions++;

        // Check for state transition "Created|Approved => Cancelled"
        if (delivery.State == DeliveryState.Cancelled && 
            (existingDelivery.State == DeliveryState.Created || existingDelivery.State == DeliveryState.Approved))
            allowedConditions++;

        if (allowedConditions > 0) {
            existingDelivery.State = delivery.State;
            await _deliveryRepository.UpdateAsync(delivery);
            return (ServiceResult.Ok, null);
        }
        return (ServiceResult.Bad, "This type of update Delivery state by Recipient is not allowed");
    }

    /// <summary>
    /// Update state of Delivery by Partner.
    /// Allowed: Approved => Completed, Created|Approved => Cancelled
    /// </summary>
    /// <param name="delivery"></param>
    /// <returns></returns>
    public async Task<(ServiceResult, string)> UpdateStateByPartner(Delivery delivery)
    {
        var allowedConditions = 0;
        var existingDelivery = await _deliveryRepository.GetByIdAsync(delivery.Id);
        if (existingDelivery == null)
        {
            return (ServiceResult.NotFound, null);
        }

        // Check if not same state
        if (delivery.State == existingDelivery.State) 
        {
            return (ServiceResult.Bad, "Partner can't update State of Delivery to the same value");
        }

        // Check for state transition "Approved => Completed"
        if (delivery.State == DeliveryState.Completed && existingDelivery.State == DeliveryState.Approved)
            allowedConditions++;

        // Check for state transition "Created|Approved => Cancelled"
        if (delivery.State == DeliveryState.Cancelled && 
            (existingDelivery.State == DeliveryState.Created || existingDelivery.State == DeliveryState.Approved))
            allowedConditions++;

        if (allowedConditions > 0) {
            existingDelivery.State = delivery.State;
            await _deliveryRepository.UpdateAsync(delivery);
            return (ServiceResult.Ok, null);
        } 
        return (ServiceResult.Bad, "This type of update Delivery state by Partner is not allowed");
    }
}
