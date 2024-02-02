namespace AuthService.Responses;

public class UserAuthResponse
{
    public string Name { get; set; }
    public string Token { get; set; }
    public int ExpiredAt { get; set; }
}