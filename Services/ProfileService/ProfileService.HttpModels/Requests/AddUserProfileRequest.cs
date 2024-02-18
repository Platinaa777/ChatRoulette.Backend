namespace ProfileService.HttpModels.Requests;

public class AddUserProfileRequest
{
    public string UserName { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
}