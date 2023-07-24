using TT.Deliveries.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TT.Deliveries.Domain.Dto;

namespace TT.Deliveries.Tests;

public class DeliveriesPartnerControllerTests : DeliveriesControllerTestsBase
{
    [Fact]
    public async Task Update_State_From_Created_To_Approved_Should_Not_Be_Allowed()
    {
        /// Arrange
        var deliveriesPartnerController = new DeliveriesPartnerController(
            _mapper, _deliveryRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInCreatedState));
 
        /// Act
        var result = await deliveriesPartnerController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Approved });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_State_From_Approved_To_Completed_Should_Be_Allowed()
    {
        /// Arrange
        var deliveriesPartnerController = new DeliveriesPartnerController(
            _mapper, _deliveryRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInApprovedState));
 
        /// Act
        var result = await deliveriesPartnerController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Completed });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_State_From_Approved_To_Cancelled_Should_Be_Allowed()
    {
        /// Arrange
        var deliveriesPartnerController = new DeliveriesPartnerController(
            _mapper, _deliveryRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInApprovedState));
 
        /// Act
        var result = await deliveriesPartnerController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Cancelled });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_State_To_Unallowed_Should_Not_Be_Allowed()
    {
        /// Arrange
        var deliveriesPartnerController = new DeliveriesPartnerController(
            _mapper, _deliveryRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInCompletedState));
 
        /// Act
        var result = await deliveriesPartnerController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Cancelled });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Get_State_Should_Return_Delivery_Details()
    {
        /// Arrange
        var deliveriesPartnerController = new DeliveriesPartnerController(
            _mapper, _deliveryRepository.Object, _deliveriesStateService);
 
        /// Act
        var result = await deliveriesPartnerController.GetState(TestData.TestDeliveryId);
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_State_Should_Return_NotFound_If_Delivery_Doesnt_Exist()
    {
        /// Arrange
        var deliveriesPartnerController = new DeliveriesPartnerController(
            _mapper, _deliveryRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestNullDelivery));
 
        /// Act
        var result = await deliveriesPartnerController.GetState(TestData.TestDeliveryId);
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }
}