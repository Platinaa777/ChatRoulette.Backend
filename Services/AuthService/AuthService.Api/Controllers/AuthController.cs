using AuthService.Api.Mappers;
using AuthService.Api.Utils;
using AuthService.Application.Commands.GenerateToken;
using AuthService.Application.Commands.LogoutUser;
using AuthService.Application.Models;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CookieOptions = Microsoft.AspNetCore.Http.CookieOptions;

namespace AuthService.Api.Controllers;

[ApiController]
[Route($"[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly RoleIdentifier _identifier;

    public AuthController(
        IMediator mediator,
        RoleIdentifier identifier)
    {
        _mediator = mediator;
        _identifier = identifier;
    }

    [HttpGet("test")]
    [Authorize]
    public ActionResult<string> GetAnswer()
    {
        return Ok("hello world");
    }

    [HttpGet("info")]
    public ActionResult<Result<UserInformationResponse>> GetUserInfo()
    {
        var role = _identifier.GetRoleFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (role is null)
            return BadRequest("Role not found in token");

        return Ok(role);
    }
    
    /// <summary>
    /// register user in the system
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register(RegisterRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());

        if (result.IsFailure)
            return BadRequest(result);
        
        return Ok(result);
    }

    /// <summary>
    /// for getting token by email and password
    /// </summary>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthTokens>> GetToken()
    {
        if (!HttpContext.Request.Headers.TryGetValue("refresh-token", out var refreshToken))
        {
            return BadRequest(ResponseMapper.NotFoundRefreshToken);
        }
        
        var result = await _mediator.Send(new GenerateTokenCommand() { RefreshToken = refreshToken.ToString()  });

        if (result.IsSuccess)
        {
            HttpContext.Response.Cookies.Append("refresh-token", result.Value.RefreshToken!,
                new CookieOptions()
                    { 
                        Expires = DateTimeOffset.Now.AddHours(2)
                    });

            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthTokens>> Login(LoginRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());

        if (result.IsSuccess)
        {
            HttpContext.Response.Cookies.Append("refresh-token", result.Value.RefreshToken!,
                new CookieOptions()
                { 
                    Expires = DateTimeOffset.Now.AddHours(2)
                });
            
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpDelete("logout")]
    public async Task<ActionResult<string>> Logout()
    {
        if (!HttpContext.Request.Headers.TryGetValue("refresh-token", out var refreshToken))
        {
            return BadRequest(ResponseMapper.NotFoundRefreshToken);
        }

        Console.WriteLine(refreshToken.ToString());
        var result = await _mediator.Send(new LogoutUserCommand() { RefreshToken = refreshToken.ToString() });

        HttpContext.Response.Cookies.Delete("refresh-token");
        
        if (result.IsFailure)
            return BadRequest(result.Error.Message);

        return Ok(string.Empty);
    }
    
    private string CutBearer(string token)
    {
        return token.Replace("Bearer ", "");
    }
}