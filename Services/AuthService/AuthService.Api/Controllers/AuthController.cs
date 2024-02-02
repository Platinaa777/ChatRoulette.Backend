using AuthService.Api.Constants;
using AuthService.Requests;
using AuthService.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route($"{AuthServiceName.AUTH}")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenHandler _authHandler;

    public AuthController(JwtTokenHandler authHandler)
    {
        _authHandler = authHandler;
    }

    [Authorize]
    [HttpGet("get")]
    public string Get() => "132";
    [HttpPost]
    public ActionResult<UserAuthResponse> Authenticate(UserAuthRequest request)
    {
        var response = _authHandler.GenerateJwtToken(request);

        if (response == null) return Unauthorized();

        return Ok(response);
    }
}