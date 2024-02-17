namespace ProfileService.Infrastructure.Repos.Models;

public class UserDb
{
    public string Id { get; set; }
    public string UserName { get; set; } = "user_name";
    public string NickName { get; set; } = "nick_name";
    public string Email { get; set; }
}