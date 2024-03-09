using AuthService.Api.Mappers;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("/user")]
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
    [HttpPost("/register")]
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
    [HttpPost("/login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(TokenRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());
        
        return Ok(result);
    }
    
    public async Task<ActionResult<
}