namespace Chat.HttpModels.HttpResponses;

public class UserJoinResponse
{
    public string Email { get; set; }
    public string ConnectionId { get; set; }
    public string RoomId { get; set; }
    public bool CreateOffer { get; set; }
}
