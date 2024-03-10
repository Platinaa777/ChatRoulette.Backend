namespace AuthService.Application.Models;

public class AuthTokens
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public AuthTokens()
    {
        
    }
    public AuthTokens(string? accessToken, string? refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}