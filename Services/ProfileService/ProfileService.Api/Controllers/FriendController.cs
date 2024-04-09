using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Utils;
using ProfileService.Application.Commands.AcceptFriendInvitation;
using ProfileService.Application.Commands.RejectFriendInvitation;
using ProfileService.Application.Commands.SendFriendInvitation;
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
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Cookies["access-token"]);

        if (email is null)
            return Unauthorized();
        
        var response = await _mediator.Send(
            new SendFriendInvitationCommand(
                invitationSenderEmail: email,
                request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut("accept-invitation-to-friends")]
    public async Task<ActionResult<Result>> AcceptInvitationToFriends([FromBody] FriendRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Cookies["access-token"]);

        if (email is null)
            return Unauthorized();
        
        var response = await _mediator.Send(
            new AcceptFriendInvitationCommand(
                invitationSenderEmail: email,
                request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut("reject-invitation-to-friends")]
    public async Task<ActionResult<Result>> RejectInvitationToFriends([FromBody] FriendRequest request)
    {
        var email = _credentialsChecker.GetEmailFromJwtHeader(Request.Cookies["access-token"]);

        if (email is null)
            return Unauthorized();
        
        var response = await _mediator.Send(
            new RejectFriendInvitationCommand(
                invitationSenderEmail: email,
                request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
}