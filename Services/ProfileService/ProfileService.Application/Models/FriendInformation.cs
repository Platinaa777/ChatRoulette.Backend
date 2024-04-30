namespace ProfileService.Application.Models;

public class FriendInformation
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Avatar { get; set; } = string.Empty;
}