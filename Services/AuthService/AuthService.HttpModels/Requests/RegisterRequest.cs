namespace AuthService.HttpModels.Requests;

public class RegisterRequest
{
    public string UserName { get; set; }
    public int? Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}