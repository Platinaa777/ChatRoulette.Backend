using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Chat.API.Configuration;
using Chat.API.Requests;
using Chat.API.Responses;
using Chat.Application.Handlers;
using Chat.Core.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
//TODO добавить регистрант токен и посмотреть как будет работать фронтенд
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
        var url = "https://api.zoom.us/v2/users/me/token";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }

        return Ok(response.Content.ReadAsStringAsync());
    }
    
    [HttpGet("join-room/{token}")]
    public async Task<IActionResult> JoinRoom(string token)
    {
        var url = "https://api.zoom.us/v2/users/me/meetings";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        var data = new { };

        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }

        return Ok(response.Content.ReadAsStringAsync());
    }

    [HttpGet("123")]
    public async Task<IActionResult> A()
    {
        return Ok("123");
    }
    

    [HttpGet("get-all-meetings/{token}")]
    public async Task<IActionResult> GetAllMeetings(string token)
    {
        var url = "https://api.zoom.us/v2/users/me/meetings";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }

        return Ok(response.Content.ReadAsStringAsync());
    }
    
    [HttpPost("generate-jwt-token")]
    public async Task<IActionResult> GenerateJwtToken([FromBody] ZoomRequest body)
    {
        var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ZoomSecret.ClientSecret));
        var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);
        
        // Calculate timestamps
        DateTime issuedAt = DateTime.UtcNow;
        DateTime expiration = issuedAt.AddHours(48); // Assuming 3 hours expiration, adjust as needed
        DateTime tokenExpiration = issuedAt.AddHours(48); // Assuming 3 hours expiration, adjust as needed
        
        // Convert timestamps to epoch format
        long expEpoch = new DateTimeOffset(expiration).ToUnixTimeSeconds();
        long iatEpoch = new DateTimeOffset(issuedAt).ToUnixTimeSeconds();
        long tokenExpEpoch = new DateTimeOffset(tokenExpiration).ToUnixTimeSeconds();

        
        var claims = new Claim[]
        {
            new Claim("appKey", ZoomSecret.ClientId),
            new Claim("sdkKey", ZoomSecret.ClientId),
            new Claim("role", body.role.ToString(), ClaimValueTypes.Integer64),
            new Claim("exp", expEpoch.ToString()),
            new Claim("iat", iatEpoch.ToString(), ClaimValueTypes.Integer64),
            new Claim("tokenExp", tokenExpEpoch.ToString()),
            new Claim("mn", body.meetingNumber, ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials);

        return Ok(new JwtTokenResponse() {Signature = new JwtSecurityTokenHandler().WriteToken(token)});
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