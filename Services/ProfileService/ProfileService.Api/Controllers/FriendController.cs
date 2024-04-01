using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.Commands.AcceptFriendInvitationCommand;
using ProfileService.Application.Commands.RejectFriendInvitationCommand;
using ProfileService.Application.Commands.SendFriendInvitationCommand;
using ProfileService.HttpModels.Requests;

namespace ProfileService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FriendController : ControllerBase
{
    private readonly IMediator _mediator;

    public FriendController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result>> AddFriend([FromBody] FriendRequest request)
    {
        var response = await _mediator.Send(
            new SendFriendInvitationCommand(request.InvitationSenderEmail, request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut]
    public async Task<ActionResult<Result>> AcceptInvitationToFriends([FromBody] FriendRequest request)
    {
        var response = await _mediator.Send(
            new AcceptFriendInvitationCommand(request.InvitationSenderEmail, request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
    
    [HttpPut]
    public async Task<ActionResult<Result>> RejectInvitationToFriends([FromBody] FriendRequest request)
    {
        var response = await _mediator.Send(
            new RejectFriendInvitationCommand(request.InvitationSenderEmail, request.InvitationReceiverEmail));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }
}