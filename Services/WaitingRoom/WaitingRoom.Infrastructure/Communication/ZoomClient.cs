using System.Text;
using Chat.Infrastructure.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WaitingRoom.Infrastructure.Responses;

namespace WaitingRoom.Infrastructure.Communication;

public class ZoomClient
{
    public const string ZoomUrl = "https://api.zoom.us/v2";
    private readonly HttpClient _client = new HttpClient();

    public async Task<string?> GetZakToken(string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        var response = await _client.GetAsync(ZoomUrl  + "/users/me/token/?type=zak");
        _client.DefaultRequestHeaders.Clear();

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        return null;
    }
    
    public async Task<MeetingsData> GetAllMeetings(string token)
    {
        var url = "https://api.zoom.us/v2/users/me/meetings";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        var response = await client.GetAsync(url);
        
        _client.DefaultRequestHeaders.Clear();;
        return JsonConvert.DeserializeObject<MeetingsData>(await response.Content.ReadAsStringAsync());
    }

    public async Task<ZoomRoomCreated> CreateRoom(string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var data = new { };

        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(ZoomUrl + "/users/me/meetings", content);
        _client.DefaultRequestHeaders.Clear();

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        JObject jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        ZoomRoomCreated room = new()
        {
            Id = (string)jObject["id"],
            HostUrl = (string)jObject["start_url"],
            JoinUrl = (string)jObject["join_url"],
            Password = (string)jObject["password"]
        };
        return room;
    }

    public async Task<ZoomInfo> GetInfoAboutRoom(string id, string token)
    {
        _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        
        var response = await _client.GetAsync(ZoomUrl + $"/meetings/{id}");

        JObject jObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        ZoomInfo room = new()
        {
            Id = (string)jObject["id"],
            HostUrl = (string)jObject["start_url"],
            JoinUrl = (string)jObject["join_url"],
            Password = (string)jObject["password"],
            IsValid = true
        };

        if (String.IsNullOrEmpty(room.Id))
        {
            room.IsValid = false;
        }
        
        _client.DefaultRequestHeaders.Clear();
        return room;
    }
}