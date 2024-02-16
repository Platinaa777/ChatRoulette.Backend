using AuthService.Api.Mappers;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet]
    public async Task<ActionResult<string>> GetSecuredData()
    {
        return Ok("This Secured Data is available only for Authenticated Users.");
    }

    [HttpGet("user")]
    public async Task<ActionResult<UserInformationResponse>> GetUserInfo(
        [FromQuery] string email,
        [FromQuery] string password)
    {
        var request = new GetUserDataRequest() { Email = email, Password = password }; 
        var result = await _mediator.Send(request.ToQuery());

        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register(RegisterRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<ActionResult<AuthenticationResponse>> GetToken(TokenRequest request)
    {
        var result = await _mediator.Send(request.ToCommand());
        
        return Ok(result);
    }
}