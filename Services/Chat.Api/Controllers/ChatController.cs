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
    
    // [HttpPost("leave-room")]
    // public async Task<ActionResult<string>> LeaveRoom([FromBody] UserLeaveRequest user)
    // {
    // }
    //
    // [HttpGet("get-all-rooms")]
    // public async Task<ActionResult<List<RoomInfoResponse>>> GetAllMeetings()
    // {
    // }
    //
    // [HttpGet("get-room/{id}")]
    // public async Task<ActionResult<RoomInfoResponse>> GetRoomById(string id)
    // {
    // }
    //
    // [HttpGet("connect")]
    // public async Task<ActionResult<bool>> CanConnectToRoom()
    // {
    // }
}