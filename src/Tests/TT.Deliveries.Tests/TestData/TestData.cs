using TT.Deliveries.Domain.Dto;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Tests;

/// <summary>
/// Testing data for tests
/// </summary>
public static class TestData
{
    public static string TestRecipientName = "Test Recipient";

    public static string TestAddress = "UK";

    public static string TestEmail = "some@mail";

    public static string TestPhone = "0123456789";

    public static Guid TestDeliveryId = Guid.NewGuid();

    public static string TestOrderNumber = "ORDER-12345";

    public static string TestSender = "Ikea";

    public static DateTime TestStartTime = new DateTime(2023, 07, 01);

    public static DateTime TestEndTime = new DateTime(2023, 07, 31);

    public static DateTime TestNewStartTime = new DateTime(2023, 07, 15);

    public static Delivery? TestNullDelivery
    { 
        get { return null; }
    }

    public static Delivery TestDelivery
    { 
        get {
            return new Delivery()
            {
                Id = TestDeliveryId,
                State = DeliveryState.Created,
                Recipient = TestRecipient,
                Order = TestOrder,
                AccessWindow = new AccessWindow(TestStartTime, TestEndTime),
            };
        }
    }

    public static Delivery TestDeliveryInCreatedState
    { 
        get {
            return GenDeliveryInState(TestDelivery, DeliveryState.Created);
        }
    }

    public static Delivery TestDeliveryInApprovedState
    { 
        get {
            return GenDeliveryInState(TestDelivery, DeliveryState.Approved);
        }
    }

    public static Delivery TestDeliveryInCompletedState
    { 
        get {
            return GenDeliveryInState(TestDelivery, DeliveryState.Completed);
        }
    }

    public static Delivery TestDeliveryInCancelledState
    { 
        get {
            return GenDeliveryInState(TestDelivery, DeliveryState.Cancelled);
        }
    }

    public static Delivery TestDeliveryInExpiredState
    { 
        get {
            return GenDeliveryInState(TestDelivery, DeliveryState.Expired);
        }
    }

    public static Recipient TestRecipient 
    { 
        get {
            return new Recipient(TestRecipientName, TestAddress, TestEmail, TestPhone);
        }
    }

    public static Order TestOrder 
    { 
        get {
            return new Order(TestOrderNumber, TestSender);
        }
    }

    public static CreateDeliveryDto TestCreateDeliveryDto
    { 
        get {
            return new CreateDeliveryDto()
            {
                Recipient = TestRecipientDto,
                Order = TestOrderDto,
                AccessWindow = new AccessWindowDto
                {
                    StartTime = TestStartTime, 
                    EndTime = TestEndTime
                },
            };
        }
    }

    public static UpdateDeliveryDto TestUpdateDeliveryDto
    { 
        get {
            return new UpdateDeliveryDto()
            {
                RecipientId = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                AccessWindow = new AccessWindowDto
                {
                    StartTime = TestStartTime, 
                    EndTime = TestEndTime
                },
            };
        }
    }    

    public static RecipientDto TestRecipientDto
    { 
        get {
            return new RecipientDto 
            { 
                Name = TestRecipientName, 
                Address = TestAddress, 
                Email = TestEmail, 
                PhoneNumber = TestPhone 
            };
        }
    }

    public static OrderDto TestOrderDto
    { 
        get {
            return new OrderDto { OrderNumber = TestOrderNumber, Sender = TestSender };
        }
    }

    public static UpdateDeliveryStateDto TestUpdateDeliveryStateDto
    { 
        get {
            return new UpdateDeliveryStateDto()
            {
                Id = Guid.NewGuid(),
                State = DeliveryState.Created
            };
        }
    }

    private static Delivery GenDeliveryInState(Delivery delivery, DeliveryState state)
    {
        delivery.State = state;
        return delivery;
    }
}
