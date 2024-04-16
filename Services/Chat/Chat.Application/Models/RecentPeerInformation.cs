namespace Chat.Application.Models;

public class RecentPeerInformation
{
    public RecentPeerInformation(string email)
    {
        Email = email;
    }
    public string Email { get; set; }
}