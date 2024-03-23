namespace AuthService.Application.Models;

public class AuthTokens
{
    public string Email { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public AuthTokens(string email)
    {
        Email = email;
    }
    public AuthTokens(string? accessToken, string? refreshToken, string email)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Email = email;
    }
}