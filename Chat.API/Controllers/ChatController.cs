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
    
    [HttpGet("find-room")]
    public async Task<IActionResult> FindRoom()
    {
        var response = await _roomHandler.JoinRoom();
        
        if (string.IsNullOrEmpty(response.AccessToken))
        {
            return BadRequest("room is not found");
        }
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Disconnect(string room, string participantSid)
    {
        var isDiconnectedResponse = await _roomHandler.DisconnectFromRoom(room, participantSid);

        if (!isDiconnectedResponse)
        {
            return BadRequest("room is not found");
        }

        return Ok(isDiconnectedResponse);
    }
}