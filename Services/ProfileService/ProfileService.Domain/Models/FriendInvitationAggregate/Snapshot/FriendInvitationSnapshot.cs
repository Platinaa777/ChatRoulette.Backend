namespace ProfileService.Domain.Models.FriendInvitationAggregate.Snapshot;

public class FriendInvitationSnapshot
{
    public string Id { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public string InvitationStatus { get; set; } = string.Empty;
    public DateTime SentAtUtc { get; set; }
}