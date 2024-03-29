using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.Commands.AddUserProfileCommand;
using ProfileService.Application.Commands.ChangeNickNameProfileCommand;
using ProfileService.Application.Models;
using ProfileService.Application.Queries;
using ProfileService.Application.Queries.GetTopUserQuery;
using ProfileService.Application.Queries.GetUserProfileQuery;
using ProfileService.Domain.Shared;
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

    [HttpGet("get-user-info")]
    public async Task<ActionResult<ProfileResponse>> GetUser([FromQuery] string email)
    {
        var result = await _mediator.Send(new GetUserProfileQuery() { Email = email });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpGet("get-top-users")]
    public async Task<ActionResult<Result<List<UserProfileInformation>>>> GetTopUsers()
    {
        var result = await _mediator.Send(new GetTopUsersQuery());

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result);
    }
    
    [HttpPost("add-user")]
    public async Task<ActionResult<Result>> AddUserProfile([FromBody] AddUserProfileRequest request)
    {
        var result = await _mediator.Send(new AddUserProfileCommand()
        {
            Id = Guid.NewGuid().ToString(),
            Age = request.Age,
            Email = request.Email,
            NickName = request.NickName
        });
        
        return Ok(result);
    }
    
    [HttpPut("change-user-nickname")]
    public async Task<ActionResult<Result>> ChangeNickName([FromBody] ChangeNicknameRequest request)
    {
        var result = await _mediator.Send(new ChangeNickNameProfileCommand()
        {
            Email = request.Email,
            NickName = request.NickName
        });
        
        return Ok(result);
    }
    
}