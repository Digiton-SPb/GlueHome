using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TT.Deliveries.Web.Api.Services;

namespace TT.Deliveries.Web.Api.BaseControllers;

/// <summary>
/// Base controller that supports executing services that returns ServiceResult and message as a result.
/// Contains methods to convert ServiceResult to ActionResult.
/// </summary>
public class ServiceResultControllerBase : ControllerBase
{
    /// <summary>
    /// Generates ActionResult from ServiceResult
    /// </summary>
    /// <param name="serviceResult"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    protected IActionResult GenActionResult(ServiceResult serviceResult, string message)
    {
        switch (serviceResult) 
        {
            case ServiceResult.Ok:
                return string.IsNullOrWhiteSpace(message) ? NoContent() : Ok(message);
            case ServiceResult.Bad:
                return BadRequest(message);
            case ServiceResult.NotFound:
                return NotFound(message);
            case ServiceResult.Unauthorized:
                return Unauthorized(message);
            default:
                return NoContent();
        }
    }

    /// <summary>
    /// Get Username from HttpContext Claims
    /// </summary>
    /// <returns></returns>
    protected string GetUserName() => 
        HttpContext?.User?.Claims
            .Where(p => p.Type == ClaimTypes.Name)
            .Select(p => p.Value)
            .FirstOrDefault();
}
