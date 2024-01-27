namespace WaitingRoom.API.HttpRequests;

public class UserLeaveRequest
{
    public string RoomId { get; set; }
    public string Email { get; set; }
}