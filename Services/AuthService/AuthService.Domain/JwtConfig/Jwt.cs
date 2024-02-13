namespace AuthService.Domain.JwtConfig;

public class Jwt
{
    public string Key { get; set; }
    public int ValidationMins { get; set; }
}