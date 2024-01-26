using Chat.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using WaitingRoom.API.HttpRequests;
using WaitingRoom.Application.Handlers;
using Mapper = AutoMapper.Mapper;

namespace WaitingRoom.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly DialogRoomHandler _dialogRoomHandler;
    private readonly Mapper  _mapper;

    public ChatController(
        DialogRoomHandler dialogRoomHandler,
        Mapper mapper)
    {
        _dialogRoomHandler = dialogRoomHandler;
        _mapper = mapper;
    }

    [HttpGet("get-all-meetings")]
    public async Task<IActionResult> GetAllMeetings()
    {
        return Ok(await _dialogRoomHandler.GetAllMeetings());
    }
    
    [HttpGet("join-room")]
    public async Task<IActionResult> JoinRoom(UserJoinRequest user)
    {
        return Ok(await _dialogRoomHandler
                            .JoinFreeRoom(_mapper.Map<UserRequest>(user)));
    }

    [HttpGet("get-meeting/{id}")]
    public async Task<IActionResult> GetRoomById(string id)
    {
        return Ok(await _dialogRoomHandler.GetRoomById(id));
    }


    [HttpGet]
    public async Task<IActionResult> LeaveRoom(string id)
    {
        return Ok(await _dialogRoomHandler.CreateRoom(id));
    }
}