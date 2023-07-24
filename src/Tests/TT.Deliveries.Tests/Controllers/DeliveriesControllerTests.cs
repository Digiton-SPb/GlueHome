using TT.Deliveries.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TT.Deliveries.Domain.Dto;

namespace TT.Deliveries.Tests;

public class DeliveryControllerTests : DeliveriesControllerTestsBase
{
    [Fact]
    public async Task GetAll_Should_Return_Deliveries_Details()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
 
        /// Act
        var result = await deliveriesController.GetAll();
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_Should_Return_Delivery_Details()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
 
        /// Act
        var result = await deliveriesController.GetById(TestData.TestDeliveryId);
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }    

    [Fact]
    public async Task GetById_Should_Return_NotFound_If_Delivery_Doesnt_Exist()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestNullDelivery));
 
        /// Act
        var result = await deliveriesController.GetById(Guid.Empty);
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_Should_Return_Created_Delivery()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
 
        /// Act
        var result = await deliveriesController.Create(TestData.TestCreateDeliveryDto);
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Update_Should_Successfully_Completed()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
 
        /// Act
        var result = await deliveriesController.Update(TestData.TestUpdateDeliveryDto);
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_State_From_Created_To_Approved_Should_Be_Allowed()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInCreatedState));
 
        /// Act
        var result = await deliveriesController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Approved });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_State_From_Approved_To_Cancelled_Should_Be_Allowed()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInApprovedState));
 
        /// Act
        var result = await deliveriesController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Cancelled });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_State_To_Unallowed_Should_Not_Be_Allowed()
    {
        /// Arrange
        var deliveriesController = new DeliveriesController(
            _mapper, _deliveryRepository.Object, _recipientRepository.Object, _deliveriesStateService);
        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDeliveryInCompletedState));
 
        /// Act
        var result = await deliveriesController.UpdateState(
            new UpdateDeliveryStateDto { Id = TestData.TestDeliveryId, State = DeliveryState.Cancelled });
 
        /// Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}