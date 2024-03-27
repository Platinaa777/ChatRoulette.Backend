using Chat.Api.Extensions.Mappers;
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
}