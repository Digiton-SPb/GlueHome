using System;
using System.Collections.Generic;
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

namespace TT.Deliveries.Web.Api.Controllers;

/// <summary>
/// Deliveries API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DeliveriesController : ServiceResultControllerBase
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IRecipientRepository _recipientRepository;
    private readonly DeliveriesStateService _deliveriesStateService;
    private IMapper Mapper { get; }

    public DeliveriesController(IMapper mapper, 
        IDeliveryRepository deliveryRepository, 
        IRecipientRepository recipientRepository,
        DeliveriesStateService deliveriesStateService) 
    {
        this.Mapper = mapper;
        _deliveryRepository = deliveryRepository;
        _recipientRepository = recipientRepository;
        _deliveriesStateService = deliveriesStateService;
    }

    /// <summary>
    /// Get All Deliveries
    /// </summary>
    /// <returns></returns>
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<Delivery>>> GetAll()
    {
        var result = await _deliveryRepository.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Get Delivery by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Delivery>>> GetById(Guid id)
    {
        var result = await _deliveryRepository.GetByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create Delivery
    /// </summary>
    /// <param name="deliveryDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Delivery>> Create([FromBody] CreateDeliveryDto deliveryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var delivery = Mapper.Map<Delivery>(deliveryDto);
        var result = await _deliveryRepository.CreateAsync(delivery);
        return Ok(result);
    }

    /// <summary>
    /// Update Delivery
    /// </summary>
    /// <param name="deliveryDto"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateDeliveryDto deliveryDto)
    {
        var delivery = Mapper.Map<Delivery>(deliveryDto);
        var existingDelivery = await _deliveryRepository.GetByIdAsync(delivery.Id);
        if (existingDelivery == null) return NotFound();

        await _deliveryRepository.UpdateAsync(delivery);
        return NoContent();
    }

    /// <summary>
    /// Delete Delivery
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(id);
        if (delivery == null) return NotFound();

        await _deliveryRepository.DeleteAsync(delivery, true);
        return NoContent();
    }

    /// <summary>
    /// Update Delivery State.
    /// Allowed transitions: Created => Approved, Created|Approved => Cancelled
    /// </summary>
    /// <param name="deliveryDto"></param>
    /// <returns></returns>
    [Authorize(Roles = Roles.User)]
    [HttpPut("UpdateState")]
    public async Task<IActionResult> UpdateState([FromBody] UpdateDeliveryStateDto deliveryDto)
    {
        var recipient = await GetRecipient();
        if (recipient == null) 
        {
            return Unauthorized();
        }

        var delivery = Mapper.Map<Delivery>(deliveryDto);

        (ServiceResult serviceResult, string message) = 
            await _deliveriesStateService.UpdateStateByRecipient(delivery, recipient);

        return GenActionResult(serviceResult, message);
    }

    private async Task<Recipient> GetRecipient() => 
        await _recipientRepository.GetByNameAsync(GetUserName());
}
