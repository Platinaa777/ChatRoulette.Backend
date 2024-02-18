using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Mappers;
using ProfileService.Application.Queries;
using ProfileService.HttpModels.Requests;
using ProfileService.HttpModels.Responses;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-user")]
    public async Task<ActionResult<UserProfileResponse>> GetUser([FromQuery] string email)
    {
        var result = await _mediator.Send(new GetUserProfileQuery() { Email = email });

        return result;
    }

    [HttpPost("create-user")]
    public async Task<ActionResult<bool>> AddUserProfile([FromBody] AddUserProfileRequest request)
    {
        var result = await _mediator.Send(request.ToAddCommand());

        return result ? Ok("Created") : BadRequest("Not created");
    }
    
    [HttpPut("update-user")]
    public async Task<ActionResult<bool>> UpdateUserProfile()
    {
        return Ok();
    }
    
}