namespace AuthService.HttpModels.Responses;

public class AuthenticationResponse
{
    public bool IsAuthenticate { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
}