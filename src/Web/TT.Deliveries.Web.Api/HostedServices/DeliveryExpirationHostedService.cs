using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TT.Deliveries.Common.Helpers;
using TT.Deliveries.Common.Services;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Dto;

namespace TT.Deliveries.Web.Api.HostedServices;

public class DeliveryExpirationHostedService : BaseSchedulingService
{
    #region Constants
    private const string DefaultCrontab = "* * * * *";
    #endregion

    #region Private Fields
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly Executor _executor;
    #endregion

    #region Ctor
    public DeliveryExpirationHostedService(IDeliveryRepository deliveryRepository, ILogger<DeliveryExpirationHostedService> logger)
        : base(DefaultCrontab, logger)
    {
        _deliveryRepository = deliveryRepository;
        _executor = new Executor(_logger);
    }
    #endregion

    #region Overrided Methods
    protected override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        cancelToken.Register(() => _logger.LogDebug("Scheduled task stopping"));

        if (!cancelToken.IsCancellationRequested)
        {
            await _executor.InvokeAsync(async () =>
            {
                try
                {
                    _logger.LogDebug("Scheduled task doing work");
                    await CheckExpired();
                    _logger.LogDebug("Scheduled task completed");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to run scheduled task");
                    return false;
                }
            });
        }
    }
    #endregion

    #region Private Methods
    private async Task CheckExpired()
    {
        // For demonstarting purposes, getting all poentially expired deliveries and that updating st`te one by one.
        // For real app I suggest to write a custom repository method that will use direct sql command, that supports by EF Core.
        // But at least it is a good idea to log what we updated.

        var expiredDeliveries = (await _deliveryRepository.GetPotentiallyExpiredAsync(DateTime.Now)).ToList();
        _logger.LogInformation($"Expired deliveries: {expiredDeliveries.Count}");
        foreach (var expiredDelivery in expiredDeliveries)
        {
            try
            {
                _logger.LogInformation($"Set Expired state for delivery: Id={expiredDelivery.Id}, " + 
                    $"Start={expiredDelivery.AccessWindow.StartTime}, End={expiredDelivery.AccessWindow.EndTime}");
                expiredDelivery.State = DeliveryState.Expired;
                await _deliveryRepository.UpdateAsync(expiredDelivery, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to set Expired state for delivery: Id={expiredDelivery.Id}");
            }            
        }
    }
    #endregion
}