namespace ProfileService.HttpModels.Responses;

public class UserProfileResponse
{
    public string NickName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string[] Preferences { get; set; } = null!;
}
