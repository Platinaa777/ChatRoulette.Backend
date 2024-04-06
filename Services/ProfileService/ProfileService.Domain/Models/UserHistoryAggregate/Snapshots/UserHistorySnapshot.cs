namespace ProfileService.Domain.Models.UserHistoryAggregate.Snapshots;

public class UserHistorySnapshot
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public int DoomSlayerPoints { get; set; }
    public int AvatarPoints { get; set; }
}