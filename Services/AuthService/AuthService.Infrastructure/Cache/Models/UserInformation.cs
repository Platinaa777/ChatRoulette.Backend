using AuthService.Domain.Models.UserAggregate.Entities;

namespace AuthService.Infrastructure.Cache.Models;

public class UserInformation
{
    public string Email { get; set; }
    public string NickName { get; set; }
    public string[] Preferences { get; set; }
    public int Age { get; set; }
}

public static class UserInformationCacheExtension
{
    public static UserInformation ToCache(this User user, string[] preferences)
        => new UserInformation()
        {
            Email = user.Email.Value,
            NickName = user.NickName.Value,
            Preferences = preferences,
            Age = user.Age.Value
        };
}