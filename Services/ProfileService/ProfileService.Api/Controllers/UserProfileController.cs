using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.Queries;
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

    [HttpGet]
    public async Task<ActionResult<UserProfileResponse>> GetUser([FromQuery] string email)
    {
        var result = await _mediator.Send(new GetUserProfileQuery() { Email = email });

        return result;
    }
}