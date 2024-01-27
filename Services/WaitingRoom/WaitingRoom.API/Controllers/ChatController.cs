using AutoMapper;
using Chat.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using WaitingRoom.API.HttpRequests;
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

    [HttpGet("get-all-meetings")]
    public async Task<IActionResult> GetAllMeetings()
    {
        return Ok(await _dialogRoomHandler.GetAllMeetings());
    }
    
    [HttpPost("join-room")]
    public async Task<IActionResult> JoinRoom([FromBody] UserJoinRequest user)
    {
        return Ok(await _dialogRoomHandler
                            .JoinFreeRoom(_mapper.Map<UserAdd>(user)));
    }

    [HttpGet("get-meeting/{id}")]
    public async Task<IActionResult> GetRoomById(string id)
    {
        return Ok(await _dialogRoomHandler.GetRoomById(id));
    }


    [HttpPost("leave-room")]
    public async Task<IActionResult> LeaveRoom([FromBody] UserLeaveRequest user)
    {
        if (await _dialogRoomHandler.LeaveRoom(_mapper.Map<UserLeave>(user)))
        {
            return Ok("user successfully leaved");
        }

        return BadRequest("some errors");
    }

    [HttpGet("connect")]
    public async Task<IActionResult> CanConnectToRoom()
    {
        return Ok(await _dialogRoomHandler.UserCanConnect());
    }
}