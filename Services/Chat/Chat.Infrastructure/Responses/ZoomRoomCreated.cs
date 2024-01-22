namespace Chat.Infrastructure.Responses;

public class ZoomRoomCreated
{
    public string Id { get; set; }
    public string HostUrl { get; set; }
    public string JoinUrl { get; set; }
    public string Password { get; set; }
    public bool IsCreated { get; set; }
}