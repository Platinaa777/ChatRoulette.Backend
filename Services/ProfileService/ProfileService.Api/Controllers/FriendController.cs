using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

    public FriendController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add-friend")]
    public async Task<ActionResult<Result>> AddFriend([FromBody] FriendRequest request)
    {
        var response = await _mediator.Send(
            new SendFriendInvitationCommand(request.InvitationSenderEmail, request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut("accept-invitation-to-friends")]
    public async Task<ActionResult<Result>> AcceptInvitationToFriends([FromBody] FriendRequest request)
    {
        var response = await _mediator.Send(
            new AcceptFriendInvitationCommand(request.InvitationSenderEmail, request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut("reject-invitation-to-friends")]
    public async Task<ActionResult<Result>> RejectInvitationToFriends([FromBody] FriendRequest request)
    {
        var response = await _mediator.Send(
            new RejectFriendInvitationCommand(request.InvitationSenderEmail, request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
}