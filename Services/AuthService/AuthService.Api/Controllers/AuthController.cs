using AuthService.Api.Mappers;
using AuthService.Application.Commands.GenerateToken;
using AuthService.Application.Commands.LogoutUser;
using AuthService.Domain.Shared;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using CookieOptions = Microsoft.AspNetCore.Http.CookieOptions;

namespace AuthService.Api.Controllers;

[ApiController]
[Route($"[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("test")]
    [Authorize]
    public async Task<ActionResult<string>> GetAnswer()
    {
        return Ok("hello world");
    }

    [HttpGet("info")]
    public async Task<ActionResult<Result<UserInformationResponse>>> GetUserInfo(
        [FromQuery] string email,
        [FromQuery] string password)
    {
        var request = new GetUserDataRequest() { Email = email, Password = password }; 
        var result = await _mediator.Send(request.ToQuery());

        return Ok(result);
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
        return Ok(result);
    }

    /// <summary>
    /// for getting token by email and password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthenticationResponse>> GetToken()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refresh-token", out string? refreshToken))
        {
            return BadRequest(ResponseMapper.NotFoundRefreshToken);
        }
        
        var result = await _mediator.Send(new GenerateTokenCommand() { RefreshToken = refreshToken });

        if (result.IsSuccess)
        {
            HttpContext.Response.Cookies.Append("refresh-token", result.Value.RefreshToken!,
                new CookieOptions()
                    { 
                        Expires = DateTimeOffset.Now.AddHours(2),
                        HttpOnly = true,
                        Secure = true, 
                        IsEssential = true
                        
                    });

            return Ok(result.ToAnswer());
        }
        
        return BadRequest(result.ToAnswer());
    }

    private string CutBearer(string token)
    {
        return token.Replace("Bearer ", "");
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());

        if (result.IsSuccess)
        {
            HttpContext.Response.Cookies.Append("refresh-token", result.Value.RefreshToken!,
                new CookieOptions()
                { 
                    Expires = DateTimeOffset.Now.AddHours(2),
                    HttpOnly = true,
                    Secure = true, 
                    IsEssential = true
                });

            return Ok(result.ToAnswer());
        }
        
        return BadRequest(result.ToAnswer());
    }

    [HttpPost("logout")]
    public async Task<ActionResult<string>> Logout()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refresh-token", out string? refreshToken))
        {
            return BadRequest(ResponseMapper.NotFoundRefreshToken);
        }

        var result = await _mediator.Send(new LogoutUserCommand() { RefreshToken = refreshToken });

        HttpContext.Response.Cookies.Delete("refresh-token");
        
        if (result.IsFailure)
            return BadRequest(result.Error.Message);

        return Ok(string.Empty);
    }
}