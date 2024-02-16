namespace AuthService.HttpModels.Requests;

public class GetUserDataRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}