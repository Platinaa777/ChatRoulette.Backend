using AuthService.Domain.Models.UserAggregate;
using MassTransit.Contracts.UserEvents;

namespace AuthService.Application.Cache.Models;

public class UserInformation
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public string[] Preferences { get; set; }
    public int Age { get; set; }
}

public static class UserInformationCacheExtension
{
    public static UserInformation ToCache(this User user)
        => new UserInformation()
        {
            Id = user.Id.Value,
            Email = user.Email.Value,
            NickName = user.NickName.Value,
            Age = user.Age.Value
        };

    public static UserFullyRegistered ToBusMessage(this UserInformation req)
        => new UserFullyRegistered(req.Id, req.Email, req.NickName, req.Age);
}