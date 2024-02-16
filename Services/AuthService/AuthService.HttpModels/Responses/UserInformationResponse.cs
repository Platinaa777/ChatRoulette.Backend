namespace AuthService.HttpModels.Responses;

public class UserInformationResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsSubmitted { get; set; }
    public string NickName { get; set; }
    public int Age { get; set; }
}