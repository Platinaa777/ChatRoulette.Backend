using AutoMapper;
using Chat.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using WaitingRoom.API.Extensions.Mappers;
using WaitingRoom.API.HttpRequests;
using WaitingRoom.API.HttpResponse;
using WaitingRoom.Application.Handlers;

namespace WaitingRoom.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly DialogRoomHandler _dialogRoomHandler;
    private readonly IMapper  _mapper;

    public ChatController(
        DialogRoomHandler dialogRoomHandler,
        IMapper mapper)
    {
        _dialogRoomHandler = dialogRoomHandler;
        _mapper = mapper;
    }
    
    [HttpPost("join-room")]
    public async Task<ActionResult<UserJoinResponse>> JoinRoom([FromBody] UserJoinRequest user)
    {
        var userAdd = user.ToUserAdd();
        var roomInfo = await _dialogRoomHandler.JoinFreeRoom(userAdd);
        var response = roomInfo.ToHttpResponse();

        return Ok(response);
    }
    
    [HttpPost("leave-room")]
    public async Task<ActionResult<string>> LeaveRoom([FromBody] UserLeaveRequest user)
    {
        await _dialogRoomHandler.LeaveRoom(user.ToUserLeave());
        return Ok("room is disbanded");
    }

    [HttpGet("get-all-rooms")]
    public async Task<ActionResult<List<RoomInfoResponse>>> GetAllMeetings()
    {
        var result = await _dialogRoomHandler.GetAllMeetings(); 
        return Ok(result.ToHttpRoomList());
    }

    [HttpGet("get-room/{id}")]
    public async Task<ActionResult<RoomInfoResponse>> GetRoomById(string id)
    {
        var room = await _dialogRoomHandler.GetRoomById(id);
        return Ok(room.ToHttpRoom());
    }
    
    [HttpGet("connect")]
    public async Task<ActionResult<bool>> CanConnectToRoom()
    {
        return Ok(await _dialogRoomHandler.UserCanConnect());
    }
}