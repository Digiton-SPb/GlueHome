using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Web.Api.Controllers;

/// <summary>
/// Deliveries API
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _repository;

    public OrdersController(IOrderRepository repository)
        => _repository = repository;

    /// <summary>
    /// Get Orders
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Create Order
    /// </summary>
    /// <param name="Order"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] Order Order)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _repository.CreateAsync(Order);
        return Ok(result);
    }

    /// <summary>
    /// Update Order
    /// </summary>
    /// <param name="Order"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Order Order)
    {
        var existingOrder = await _repository.GetByIdAsync(Order.Id);
        if (existingOrder != null) 
        {
            await _repository.UpdateAsync(Order);
            return NoContent();
        }
        return NotFound();
    }

    /// <summary>
    /// Delete Order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Order = await _repository.GetByIdAsync(id);
        if (Order != null) 
        {
            await _repository.DeleteAsync(Order, true);
            return NoContent();
        }
        return NotFound();
    }
}
