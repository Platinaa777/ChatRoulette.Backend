using Chat.API.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly TwilioSettings _twilioSettings;
    
    public ChatController(IOptions<TwilioSettings> settings)
    {
        _twilioSettings = settings.Value;
        TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
    }

    private string name = "";
    [HttpGet("create-room")]
    public IActionResult CreateRoom()
    {
        var nameRoom = Guid.NewGuid().ToString();
        name = nameRoom;
        var room = RoomResource.Create(uniqueName: nameRoom);

        return Ok(new {NameRoom=nameRoom, Room=room.Sid, Duration=room.Duration,room.Status,room.Type });
    }
    
    [HttpGet("get-token")]
    public IActionResult GetToken()
    {
        // These are specific to Video
        const string identity = "user";

        // Create a Video grant for this token
        var grant = new VideoGrant();
        grant.Room = "cool room";

        var grants = new HashSet<IGrant> { grant };

        // Create an Access Token generator
        var token = new Token(
            _twilioSettings.AccountSid,
            _twilioSettings.ApiKey,
            _twilioSettings.AuthToken,
            identity: identity,
            grants: grants);

        return Ok(token.ToJwt());
    }

    [HttpGet("find-room")]
    public IActionResult FindRoom()
    {
        return Ok();
    }

    [HttpGet("join-room")]
    public IActionResult JoinRoom()
    {
        return Ok("RM0f904a37c163b299cb6d9aab1cd105e5");
    }
}