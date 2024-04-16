using System.Security.AccessControl;

namespace ProfileService.Application.Models;

public class AchievementResponse
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}