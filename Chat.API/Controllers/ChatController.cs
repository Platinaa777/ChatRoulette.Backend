using Chat.Application.Handlers;
using Chat.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly RoomHandler _roomHandler;
    
    public ChatController(RoomHandler roomHandler)
    {
        _roomHandler = roomHandler;
    }

    
    [HttpGet("create-room")]
    public async Task<IActionResult> CreateRoom()
    {
        return Ok (await _roomHandler.CreateRoom());
    }
    
    [HttpGet("find-room")]
    public async Task<IActionResult> FindRoom()
    {
        var tokenRoom = await _roomHandler.JoinRoom();
        
        if (string.IsNullOrEmpty(tokenRoom))
        {
            return BadRequest("room is not found");
        }
        return Ok(tokenRoom);
    }
}