using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TT.Deliveries.DataAccess.EF.Repositories;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Tests.Repositories;

public class OrderRepositoryTests
{
    private ILogger<Order> _logger = Mock.Of<ILogger<Order>>();
    private DeliveriesContext Context() => 
        new DeliveriesContext(new DbContextOptionsBuilder<DeliveriesContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);  

    [Fact]
    public void CreateTest()
    {
        using var context = Context();
        var repository = new OrderRepository(context, _logger);

        // Arrange
        var testOrder = TestData.TestOrder;

        // Act
        repository.CreateAsync(testOrder).Wait();

        // Assert
        Assert.Equal(1, context.Orders.Count());
        var order = context.Orders.FirstOrDefault();
        Assert.NotNull(order);
        Assert.Equal(testOrder.OrderNumber, order?.OrderNumber);    
    }

    [Fact]
    public void UpdateTest()
    {
        using var context = Context();
        var repository = new OrderRepository(context, _logger);
        var testOrder = TestData.TestOrder;
        var newOrderNumber = testOrder.OrderNumber + "-NEW";
        repository.CreateAsync(testOrder).Wait(); 
        var order = context.Orders.FirstOrDefault();

        // Arrange
        testOrder.OrderNumber = newOrderNumber;

        // Act
        repository.UpdateAsync(testOrder).Wait();

        // Assert
        Assert.Equal(1, context.Orders.Count());
        order = context.Orders.FirstOrDefault();
        Assert.NotNull(order);
        Assert.Equal(newOrderNumber, order?.OrderNumber);
    }

    [Fact]
    public void CreateUpdateDeleteTest()
    {
        using var context = Context();
        var repository = new OrderRepository(context, _logger);

        // Arrange
        var testOrder = TestData.TestOrder;
        
        // Act
        repository.CreateAsync(testOrder).Wait();
        repository.DeleteAsync(testOrder).Wait();

        // Assert
        Assert.Equal(0, context.Orders.Count());
    }
}
