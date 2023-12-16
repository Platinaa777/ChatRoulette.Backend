namespace Chat.Application.Responses;

public class JoinResponse
{
    public string AccessToken { get; set; }
    public string RoomName { get; set; }
    public string Message { get; set; }
    public bool IsValid { get; set; }
}