using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TT.Deliveries.DataAccess.EF.Repositories;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Tests.Repositories;

public class RecipientRepositoryTests
{
    private ILogger<Recipient> _logger = Mock.Of<ILogger<Recipient>>();
    private DeliveriesContext Context() => 
        new DeliveriesContext(new DbContextOptionsBuilder<DeliveriesContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);  

    [Fact]
    public void CreateTest()
    {
        using var context = Context();
        var repository = new RecipientRepository(context, _logger);

        // Arrange
        var testRecipient = TestData.TestRecipient;

        // Act
        repository.CreateAsync(testRecipient).Wait();

        // Assert
        Assert.Equal(1, context.Recipients.Count());
        var recipient = context.Recipients.FirstOrDefault();
        Assert.NotNull(recipient);
        Assert.Equal(testRecipient.Name, recipient?.Name);    
    }

    [Fact]
    public void UpdateTest()
    {
        using var context = Context();
        var repository = new RecipientRepository(context, _logger);
        var testRecipient = TestData.TestRecipient;
        var newAddress = testRecipient.Address + "-NEW";
        repository.CreateAsync(testRecipient).Wait(); 
        var recipient = context.Recipients.FirstOrDefault();

        // Arrange
        testRecipient.Address = newAddress;

        // Act
        repository.UpdateAsync(testRecipient).Wait();

        // Assert
        Assert.Equal(1, context.Recipients.Count());
        recipient = context.Recipients.FirstOrDefault();
        Assert.NotNull(recipient);
        Assert.Equal(newAddress, recipient?.Address);
    }

    [Fact]
    public void CreateUpdateDeleteTest()
    {
        using var context = Context();
        var repository = new RecipientRepository(context, _logger);

        // Arrange
        var testRecipient = TestData.TestRecipient;
        
        // Act
        repository.CreateAsync(testRecipient).Wait();
        repository.DeleteAsync(testRecipient).Wait();

        // Assert
        Assert.Equal(0, context.Recipients.Count());
    }
}
