using Moq;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Models;
using AutoMapper;
using TT.Deliveries.Web.Api.Services;

namespace TT.Deliveries.Tests;

public class DeliveriesControllerTestsBase
{
    protected IMapper _mapper;
    protected readonly Mock<IDeliveryRepository> _deliveryRepository;
    protected readonly Mock<IRecipientRepository> _recipientRepository;
    protected readonly DeliveriesStateService _deliveriesStateService;

    public DeliveriesControllerTestsBase()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddMaps(new[] {
            "TT.Deliveries.Web.Api"
        }));
        _mapper = mapperConfig.CreateMapper();

        _deliveryRepository = new Mock<IDeliveryRepository>();
        _recipientRepository = new Mock<IRecipientRepository>();
        _deliveriesStateService = new DeliveriesStateService(_deliveryRepository.Object);

        _deliveryRepository
            .Setup(_ => _.GetAllAsync(new CancellationToken()))
            .Returns(Task.FromResult((new List<Delivery> { TestData.TestDelivery }).AsEnumerable()));

        _deliveryRepository
            .Setup(_ => _.GetByIdAsync(It.IsAny<Guid>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDelivery));

        _deliveryRepository
            .Setup(_ => _.CreateAsync(It.IsAny<Delivery>(), It.IsAny<bool>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestDelivery));

        _deliveryRepository
            .Setup(_ => _.UpdateAsync(It.IsAny<Delivery>(), It.IsAny<bool>(), new CancellationToken()))
            .Returns(Task.CompletedTask);

        _deliveryRepository
            .Setup(_ => _.DeleteAsync(It.IsAny<Delivery>(), It.IsAny<bool>(), new CancellationToken()))
            .Returns(Task.CompletedTask);

        _recipientRepository
            .Setup(_ => _.GetByNameAsync(It.IsAny<string>(), new CancellationToken()))
            .Returns(Task.FromResult(TestData.TestRecipient));            
    }
}