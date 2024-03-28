namespace ProfileService.HttpModels.Requests;

public class AddUserProfileRequest
{
    public string NickName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}