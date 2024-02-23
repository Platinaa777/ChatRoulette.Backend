namespace ProfileService.HttpModels.Responses;

public class UserProfileResponse
{
    public string NickName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string[] Preferences { get; set; }
}
