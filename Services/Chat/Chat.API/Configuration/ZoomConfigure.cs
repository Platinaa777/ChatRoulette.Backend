using System.Text;
using Chat.Core.Secrets;

namespace Chat.API.Configuration;

public static class ZoomConfigure
{
    public static async Task<string?> GenerateAccessToken()
    {
        HttpClient client = new HttpClient();
        var url = "https://zoom.us/oauth/token";
        
        var authorizationHeader = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{ZoomSecret.ClientId}:{ZoomSecret.ClientSecret}"));
        
        var requestData = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "account_credentials"),
            new("account_id", ZoomSecret.AccountId),
        };
        
        var requestContent = new FormUrlEncodedContent(requestData);
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + authorizationHeader);
        var response = await client.PostAsync(url, requestContent);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        return null;
    }
}