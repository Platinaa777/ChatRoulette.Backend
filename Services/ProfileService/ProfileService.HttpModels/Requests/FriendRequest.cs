namespace ProfileService.HttpModels.Requests;

public class FriendRequest
{
    public string InvitationSenderEmail { get; set; }
    public string InvitationReceiverEmail { get; set; }
}