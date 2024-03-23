namespace AuthService.HttpModels.Responses;

public class AuthenticationResponse
{
    public string Email { get; set; }
    public bool IsAuthenticate { get; set; }
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public ErrorInfo? Error { get; set; }
    
    public AuthenticationResponse(
        bool isAuthenticate,
        string email,
        ErrorInfo? error)
    {
        IsAuthenticate = isAuthenticate;
        Email = email;
        Error = error;
    }
    
    public AuthenticationResponse(
        bool isAuthenticate,
        string email,
        string accessToken,
        string refreshToken)
    {
        IsAuthenticate = isAuthenticate;
        Email = email;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}