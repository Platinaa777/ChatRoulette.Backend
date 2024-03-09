namespace AuthService.Application.Security.Models;

public class TokenPair
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}