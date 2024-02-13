using AuthService.Application.Services;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route($"[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSecuredData()
    {
        return Ok("This Secured Data is available only for Authenticated Users.");
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register(RegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<ActionResult<AuthenticationResponse>> GetToken(TokenRequest request)
    {
        return Ok(await _userService.GetTokenAsync(request));
    }
}