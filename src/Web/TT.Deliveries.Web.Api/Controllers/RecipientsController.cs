using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Domain.Models;

namespace TT.Deliveries.Web.Api.Controllers;

/// <summary>
/// Recipients API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecipientsController : ControllerBase
{
    private readonly IRecipientRepository _repository;

    public RecipientsController(IRecipientRepository repository)
        => _repository = repository;

    /// <summary>
    /// Get Recipients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recipient>>> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Create Recipient
    /// </summary>
    /// <param name="Recipient"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Recipient>> Create([FromBody] Recipient Recipient)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _repository.CreateAsync(Recipient);
        return Ok(result);
    }

    /// <summary>
    /// Update Recipient
    /// </summary>
    /// <param name="Recipient"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Recipient Recipient)
    {
        var existingRecipient = await _repository.GetByIdAsync(Recipient.Id);
        if (existingRecipient != null) 
        {
            await _repository.UpdateAsync(Recipient);
            return NoContent();
        }
        return NotFound();
    }

    /// <summary>
    /// Delete Recipient
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        var Recipient = await _repository.GetByIdAsync(id);
        if (Recipient != null) 
        {
            await _repository.DeleteAsync(Recipient, true);
            return NoContent();
        }
        return NotFound();
    }
}
