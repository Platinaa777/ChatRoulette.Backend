namespace Chat.Application.Requests;

public class UserLeave
{
    public string RoomId { get; set; }
    public string Email { get; set; }
}