namespace Chat.HttpModels.HttpRequests;

public class UserLeaveRequest
{
    public string RoomId { get; set; }
    public string Email { get; set; }
    public string ConnectionId { get; set; }
}