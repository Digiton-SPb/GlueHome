using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TT.Deliveries.DataAccess.EF.Repositories;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Tests.Repositories;

public class DeliveryRepositoryTests
{
    private ILogger<Delivery> _logger = Mock.Of<ILogger<Delivery>>();
    private DeliveriesContext Context() => 
        new DeliveriesContext(new DbContextOptionsBuilder<DeliveriesContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);  

    [Fact]
    public void CreateTest()
    {
        using var context = Context();
        var repository = new DeliveryRepository(context, _logger);

        // Arrange
        var testDelivery = TestData.TestDelivery;

        // Act
        repository.CreateAsync(testDelivery).Wait();

        // Assert
        Assert.Equal(1, context.Deliveries.Count());
        var delivery = context.Deliveries.FirstOrDefault();
        Assert.NotNull(delivery);
        Assert.Equal(TestData.TestStartTime, delivery?.AccessWindow.StartTime);    
    }

    [Fact]
    public void UpdateTest()
    {
        using var context = Context();
        var repository = new DeliveryRepository(context, _logger);
        var testDelivery = TestData.TestDelivery;
        repository.CreateAsync(testDelivery).Wait(); 
        var delivery = context.Deliveries.FirstOrDefault();

        // Arrange
        testDelivery.AccessWindow.StartTime = TestData.TestNewStartTime;

        // Act
        repository.UpdateAsync(testDelivery).Wait();

        // Assert
        Assert.Equal(1, context.Deliveries.Count());
        delivery = context.Deliveries.FirstOrDefault();
        Assert.NotNull(delivery);
        Assert.Equal(TestData.TestNewStartTime, delivery?.AccessWindow.StartTime);
    }

    [Fact]
    public void CreateUpdateDeleteTest()
    {
        using var context = Context();
        var repository = new DeliveryRepository(context, _logger);

        // Arrange
        var testDelivery = TestData.TestDelivery;
        
        // Act
        repository.CreateAsync(testDelivery).Wait();
        repository.DeleteAsync(testDelivery).Wait();

        // Assert
        Assert.Equal(0, context.Deliveries.Count());
    }
}
