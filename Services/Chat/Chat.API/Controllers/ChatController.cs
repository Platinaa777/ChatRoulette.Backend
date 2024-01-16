using System.Collections.Specialized;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    [HttpGet("get-token")]
    public async Task<IActionResult> GetToken()
    {
        HttpClient client = new HttpClient();
        
        var url = "https://zoom.us/oauth/token";
        var accountId = "KqadFh-fRRadHV1A6N7nkg";
        var clientId = "X52c32GFQo2nsNxlD8EkjQ";
        var clientSecret = "y26ArcTZOin757Fel1AtrkyBpyBDk53A";
        var authorizationHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
        
        var requestData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", "account_credentials"),
            new KeyValuePair<string, string>("account_id", accountId),
        };
        
        var requestContent = new FormUrlEncodedContent(requestData);
        
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + authorizationHeader);
        
        var response = await client.PostAsync(url, requestContent);
        var responseContent = "";
        if (response.IsSuccessStatusCode)
        {
            responseContent = await response.Content.ReadAsStringAsync();
            return Ok(responseContent);
        }

        return BadRequest();
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