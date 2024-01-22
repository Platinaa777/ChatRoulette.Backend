namespace Chat.Infrastructure.Responses;

public class ZoomInfo
{
    public string Id { get; set; }
    public string HostUrl { get; set; }
    public string JoinUrl { get; set; }
    public string Password { get; set; }
    public bool IsValid { get; set; }
}