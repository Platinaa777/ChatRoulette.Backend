namespace ProfileService.Domain.Models.UserProfileAggregate.Snapshot;

public class UserProfileSnapshot
{
    public string Id { get; init; } = string.Empty;
    public string NickName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int Age { get; init; }
    public int Rating { get; init; }
    public string FriendIds { get; init; } = string.Empty;
}