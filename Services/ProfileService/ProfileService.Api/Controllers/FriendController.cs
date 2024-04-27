using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Utils;
using ProfileService.Application.Commands.AcceptFriendInvitation;
using ProfileService.Application.Commands.RejectFriendInvitation;
using ProfileService.Application.Commands.SendFriendInvitation;
using ProfileService.Application.Queries.GetInvitations;
using ProfileService.HttpModels.Requests;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("friend")]
public class FriendController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CredentialsChecker _credentialsChecker;

    public FriendController(
        IMediator mediator,
        CredentialsChecker credentialsChecker)
    {
        _mediator = mediator;
        _credentialsChecker = credentialsChecker;
    }

    [HttpPost("add-friend")]
    public async Task<ActionResult<Result>> AddFriend([FromBody] FriendRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var response = await _mediator.Send(
            new SendFriendInvitationCommand(
                invitationSenderEmail: email,
                invitationReceiverEmail: request.NewFriendEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut("accept-invitation-to-friends")]
    public async Task<ActionResult<Result>> AcceptInvitationToFriends([FromBody] AcceptFriendRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var response = await _mediator.Send(
            new AcceptFriendInvitationCommand(
                AnswerEmail: request.AcceptNewFriendEmail,
                SenderEmail: email));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut("reject-invitation-to-friends")]
    public async Task<ActionResult<Result>> RejectInvitationToFriends([FromBody] RejectFriendRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();
        
        var response = await _mediator.Send(
            new RejectFriendInvitationCommand(
                AnswerEmail: request.RejectPersonEmail ,
                SenderEmail: email));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("get-invitations")]
    public async Task<ActionResult<Result>> GetInvitationsList()
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Replace("Bearer ", string.Empty));

        if (email is null)
            return Unauthorized();

        var response = await _mediator.Send(new GetInvitationsQuery(email));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
}