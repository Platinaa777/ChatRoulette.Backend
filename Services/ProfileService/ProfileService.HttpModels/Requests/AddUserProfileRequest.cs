namespace ProfileService.HttpModels.Requests;

public class AddUserProfileRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}