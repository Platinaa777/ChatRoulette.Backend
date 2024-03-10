namespace AuthService.HttpModels.Responses;

public class AuthenticationResponse
{
    public bool IsAuthenticate { get; set; }
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    
    public AuthenticationResponse()
    {
        
    }

    public AuthenticationResponse(bool isAuthenticate)
    {
        IsAuthenticate = isAuthenticate;
    }
}