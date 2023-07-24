using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Dto;
using TT.Deliveries.Domain.Models;
using TT.Deliveries.Web.Api.Constants;
using TT.Deliveries.Web.Api.Services;
using TT.Deliveries.Web.Api.BaseControllers;
using System;

namespace TT.Deliveries.Web.Api.Controllers;

/// <summary>
/// Partner API for get or update Deliveries state by Partners
/// </summary>
[ApiController]
[Route("partner-api/Deliveries")]
public class DeliveriesPartnerController : ServiceResultControllerBase
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly DeliveriesStateService _deliveriesStateService;
    private IMapper Mapper { get; }

    public DeliveriesPartnerController(IMapper mapper, 
        IDeliveryRepository deliveryRepository, 
        DeliveriesStateService deliveriesStateService) 
    {
        this.Mapper = mapper;
        _deliveryRepository = deliveryRepository;
        _deliveriesStateService = deliveriesStateService;
    }

    /// <summary>
    /// Get Delivery State
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Roles = Roles.Partner)]
    [HttpGet]
    public async Task<IActionResult> GetState(Guid id)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(id);
        if (delivery == null) return NotFound();

        return Ok(delivery);
    }

    /// <summary>
    /// Update Delivery State.
    /// Allowed transitions: Approved => Completed, Created|Approved => Cancelled
    /// </summary>
    /// <param name="deliveryDto"></param>
    /// <returns></returns>
    [Authorize(Roles = Roles.Partner)]
    [HttpPut]
    public async Task<IActionResult> UpdateState([FromBody] UpdateDeliveryStateDto deliveryDto)
    {
        var delivery = Mapper.Map<Delivery>(deliveryDto);
        var existingDelivery = await _deliveryRepository.GetByIdAsync(delivery.Id);
        if (existingDelivery == null) return NotFound();

        (ServiceResult serviceResult, string message) = 
            await _deliveriesStateService.UpdateStateByPartner(delivery);

        return GenActionResult(serviceResult, message);
    }
}
