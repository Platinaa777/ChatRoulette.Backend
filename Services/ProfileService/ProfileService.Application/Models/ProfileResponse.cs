using ProfileService.Application.Queries.GetUserProfile;

namespace ProfileService.Application.Models;

public class ProfileResponse
{
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime BirthDateUtc { get; init; }
    public int Rating { get; init; }
    public List<FriendInformation> Friends { get; init; } = new();
    public List<AchivementResponse> Achivements { get; set; } = new();
}