using Chat.Application.Handlers;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Proxy.V1.Service.Session;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using ParticipantResource = Twilio.Rest.Insights.V1.Room.ParticipantResource;

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
        // var response = await _roomHandler.JoinRoom();
        
        // if (string.IsNullOrEmpty(response.AccessToken))
        // {
            // return BadRequest("room is not found");
        // }
        // return Ok(response);
        return Ok("dsdasdas");
    }

    [HttpGet("disconnect-room")]
    public async Task<IActionResult> Disconnect(string room, string participantSid)
    {
        var isDisconnectedResponse = await _roomHandler.DisconnectFromRoom(room, participantSid);

        if (!isDisconnectedResponse)
        {
            return BadRequest("room is not found");
        }

        return Ok(isDisconnectedResponse);
    }


    private List<String> rooms = new();
    [HttpGet("create-room")]
    public async Task<IActionResult> CreateRoom()
    {
        var room = await _roomHandler.CreateRoom(); 
        rooms.Add(room);

        return Ok(room);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(rooms);
    }
}