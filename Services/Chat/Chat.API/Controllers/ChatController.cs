using System.Collections.Specialized;
using System.Text;
using Chat.API.Configuration;
using Chat.API.Requests;
using Chat.Application.Handlers;
using Chat.Core.Secrets;
using Microsoft.AspNetCore.Mvc;

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
            Console.WriteLine(await response.Content.ReadAsStringAsync());
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
        
        var requestData = new List<KeyValuePair<string, string>> { new("schedule_for", "miroshnichenkodenis2004@gmail.com") };
        var data = new
        {
            
        };

        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return Ok(await response.Content.ReadAsStringAsync());
        }

        return Ok(response.Content.ReadAsStringAsync());
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
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return Ok(await response.Content.ReadAsStringAsync());
        }

        return Ok(response.Content.ReadAsStringAsync());
    }
    

    [HttpGet("leave-room")]
    public async Task<IActionResult> LeaveRoom()
    {
        return Ok();
    }
    
    
    [HttpGet("generate-token")]
    public async Task<string?> GetToken()
    {
        if (_token == "")
        {
            _token = await ZoomConfigure.GenerateAccessToken();
        }
        
        return _token;
    }

    [HttpGet("get-info-about-me/{token}")]
    public async Task<IActionResult> GetInfoAboutMe(string token)
    {
        var url = "https://api.zoom.us/v2/users/me";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return Ok(await response.Content.ReadAsStringAsync());
        }

        return Ok();
    }
}