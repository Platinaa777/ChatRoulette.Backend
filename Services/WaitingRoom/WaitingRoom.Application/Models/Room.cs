namespace Chat.Application.Models;

public class Room
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public UserInfo? Host { get; set; } = null;
    public UserInfo? Participant { get; set; } = null;
}