using Chat.GrpcClient;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Utils;
using ProfileService.Application.Commands.AddUserProfile;
using ProfileService.Application.Commands.ChangeUserNameProfile;
using ProfileService.Application.Models;
using ProfileService.Application.Queries.GetPeersInformation;
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
    private readonly IChatGrpcClient _chatGrpcClient;

    public UserProfileController(
        IMediator mediator,
        CredentialsChecker credentialsChecker,
        IChatGrpcClient chatGrpcClient)
    {
        _mediator = mediator;
        _credentialsChecker = credentialsChecker;
        _chatGrpcClient = chatGrpcClient;
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

        if (result.IsFailure)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpPut("change-username")]
    public async Task<ActionResult<Result>> ChangeUserName([FromBody] ChangeUserNameRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var result = await _mediator.Send(new ChangeUserNameProfileCommand()
        {
            Email = email,
            UserName = request.UserName
        });

        if (result.IsFailure)
            return BadRequest(result);
        
        return Ok(result);
    }

    [HttpGet("get-recent-users")]
    public async Task<ActionResult<Result<UserProfileInformation>>> GetRecentUsers()
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();

        var peerEmails = await _chatGrpcClient.GetRecentPeers(email);

        if (peerEmails.Count == 0)
            return BadRequest("No peer history");

        var result = await _mediator.Send(new GetPeersInformationQuery()
        {
            PeerEmails = peerEmails.Select(x => x.Email).ToList()
        });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
}