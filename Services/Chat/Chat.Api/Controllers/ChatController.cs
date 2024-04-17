using Chat.Api.Extensions.Mappers;
using Chat.Application.Queries.GetRecentPeers;
using Chat.Grpc;
using Chat.HttpModels.HttpRequests;
using Chat.HttpModels.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("join-room")]
    public async Task<ActionResult<UserJoinResponse>> JoinRoom([FromBody] UserJoinRequest user)
    {

        var response = await _mediator.Send(user.ToCommand());

        return Ok(response);
    }

    [HttpGet("get-recent-peers/{email}")]
    public async Task<ActionResult<List<string>>> GetRecentPeers(string email)
    {
        var result = await _mediator.Send(new GetRecentPeersQuery(email));

        if (result.IsFailure)
            return BadRequest();

        var list = new List<string>();

        foreach (var peer in result.Value)
            list.Add(peer.Email);

        return list;
    }
}