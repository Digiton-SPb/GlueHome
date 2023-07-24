using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.Web.Api.Constants;

/// <summary>
/// Auth controller. For demonstrating purposes need to provide only userName without any password to get token.
/// </summary>
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const int BearerTimeoutInMinutes = 5;
    private readonly IRecipientRepository _recipientRepository;

    public AuthController(IRecipientRepository recipientRepository)
        => _recipientRepository = recipientRepository;

    /// <summary>
    /// Login User
    /// </summary>
    /// <param name="userName"></param>
    [HttpPost("User")]
    public async Task<IActionResult> LoginUser(string userName)
    {
        if (userName == null) return BadRequest();
        
        var recipient = await _recipientRepository.GetByNameAsync(userName);
        if (recipient != null)
        {
            return Ok(new { Token = GenToken(userName, new string[] { Roles.User }) });
        }
        return Unauthorized();
    }

    /// <summary>
    /// Login Partner
    /// </summary>
    /// <param name="partnerName"></param>
    [HttpPost("Partner")]
    public IActionResult LoginPartner(string partnerName)
    {
        if (partnerName == null) return BadRequest();

        // for demo purposes, accepting any partnerName
        return Ok(new { Token = GenToken(partnerName, new string[] { Roles.Partner }) });
    }

    /// <summary>
    /// Verify if authenticated
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("Verify")]
    public IActionResult VerifyAuth()
    {
        var userName = HttpContext.User.Claims
            .Where(p => p.Type == ClaimTypes.Name)
            .Select(p => p.Value)
            .FirstOrDefault();

        return Ok($"Hello {userName}");
    }

    private string GenToken(string userName, IList<string> roles)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DeliveriesSecurityKey"));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, userName) };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(BearerTimeoutInMinutes),
            signingCredentials: signinCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}