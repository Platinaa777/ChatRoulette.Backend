using AuthService.Api.Mappers;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

    [HttpGet("info")]
    public async Task<ActionResult<UserInformationResponse>> GetUserInfo(
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
    public async Task<ActionResult<AuthenticationResponse>> GetToken(TokenRequest request)
    {
        if (!HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues accessToken))
        {
            return Unauthorized("No access token was passed");
        }
        
        var result = await _mediator.Send(request.ToCommand(CutBearer(accessToken!).Trim()));
        
        return Ok(result);
    }

    private string CutBearer(string token)
    {
        return token.Replace("Bearer ", "");
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
    {
        var response = await _mediator.Send(request.ToCommand());

        if (response.RefreshToken is null)
            return BadRequest(new AuthenticationResponse(false));
        
        return Ok(new AuthenticationResponse()
        {
            AccessToken = response.AccessToken!,
            RefreshToken = response.RefreshToken!,
            IsAuthenticate = true
        });
    }
}