using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WaitingRoom.API.Configuration;
using WaitingRoom.API.Requests;
using WaitingRoom.API.Responses;
using WaitingRoom.Application.Handlers;

namespace WaitingRoom.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly DialogRoomHandler _dialogRoomHandler;
    private string _token = "";
    public ChatController(DialogRoomHandler dialogRoomHandler)
    {
        _dialogRoomHandler = dialogRoomHandler;
    }

    [HttpGet("get-zak-token/{token}")]
    public async Task<IActionResult> GetZakToken(string token)
    {
        return Ok(await _dialogRoomHandler.GetZakToken(token));
    }
    
    [HttpGet("join-room/{token}")]
    public async Task<IActionResult> JoinRoom(string token)
    {
        return Ok(await _dialogRoomHandler.CreateRoom(token));
    }

    [HttpGet("get-meeting/{id}")]
    public async Task<IActionResult> GetMeeting(string id, string token)
    {
        return Ok(await _dialogRoomHandler.GetMeetingById(id, token));
    }
    

    [HttpGet("get-all-meetings/{token}")]
    public async Task<IActionResult> GetAllMeetings(string token)
    {
        return Ok(await _dialogRoomHandler.GetAllMeetings(token));
    }
    
    [HttpPost("generate-jwt-token")]
    public async Task<IActionResult> GenerateJwtToken([FromBody] ZoomRequest body)
    {
        return Ok(new JwtTokenResponse() {Signature = JwtConfigure.GenerateJwtToken(body)});
    }
    
    [HttpGet("generate-access-token")]
    public async Task<string?> GetToken()
    {
        if (_token == "")
        {
            _token = await ZoomConfigure.GenerateAccessToken();
        }
        
        return _token;
    }
}