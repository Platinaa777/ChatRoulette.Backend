using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Utils;
using ProfileService.Application.Commands.AddUserProfile;
using ProfileService.Application.Commands.ChangeNickNameProfile;
using ProfileService.Application.Models;
using ProfileService.Application.Queries.GetTopUsers;
using ProfileService.Application.Queries.GetUserProfile;
using ProfileService.HttpModels.Requests;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("profile")]
public class UserProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CredentialsChecker _credentialsChecker;

    public UserProfileController(
        IMediator mediator,
        CredentialsChecker credentialsChecker)
    {
        _mediator = mediator;
        _credentialsChecker = credentialsChecker;
    }

    [HttpGet("get-user-info")]
    public async Task<ActionResult<ProfileResponse>> GetUser()
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();

        var result = await _mediator.Send(new GetUserProfileQuery(email));

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpGet("get-top-users/{count:int}")]
    public async Task<ActionResult<Result<List<UserProfileInformation>>>> GetTopUsers([FromRoute] int count)
    {
        var result = await _mediator.Send(new GetTopUsersQuery(count));

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
            BirthDateUtc = request.BirthDate,
            Email = request.Email,
            UserName = request.UserName
        });
        
        return Ok(result);
    }
    
    [HttpPut("change-user-nickname")]
    public async Task<ActionResult<Result>> ChangeNickName([FromBody] ChangeNicknameRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var result = await _mediator.Send(new ChangeNickNameProfileCommand()
        {
            Email = email,
            NickName = request.UserName
        });
        
        return Ok(result);
    }
    
}