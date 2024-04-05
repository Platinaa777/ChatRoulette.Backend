using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.Commands.AddUserProfile;
using ProfileService.Application.Commands.ChangeNickNameProfile;
using ProfileService.Application.Models;
using ProfileService.Application.Queries.GetTopUsers;
using ProfileService.Application.Queries.GetUserProfile;
using ProfileService.HttpModels.Requests;

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
            return BadRequest(result);

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