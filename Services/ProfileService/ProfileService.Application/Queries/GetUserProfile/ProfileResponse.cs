namespace ProfileService.Application.Queries.GetUserProfile;

public class ProfileResponse
{
    public string NickName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int Age { get; init; }
    public int Rating { get; init; }
    public List<string> FriendIds { get; init; } = new();
    public List<AchivementResponse> Achivements { get; set; }
}