namespace AdminService.Application.Models;

public class FeedbackInformation
{
    public string Id { get; set; } = "";
    public string Content { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsWatched { get; set; }
}