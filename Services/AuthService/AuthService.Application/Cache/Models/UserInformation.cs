using AuthService.Domain.Models.UserAggregate;
using MassTransit.Contracts.UserEvents;

namespace AuthService.Application.Cache.Models;

public class UserInformation
{
    public string Id { get; set; } = "";
    public string Email { get; set; } = "";
    public string UserName { get; set; } = "";
    public DateTime BirthDateUtc { get; set; }
}

public static class UserInformationCacheExtension
{
    public static UserInformation ToCache(this User user)
        => new()
        {
            Id = user.Id.Value,
            Email = user.Email.Value,
            UserName = user.UserName.Value,
            BirthDateUtc = user.BirthDateUtc.Value
        };

    public static UserFullyRegistered ToBusMessage(this UserInformation req)
        => new(
            req.Id,
            req.Email,
            req.UserName,
            req.BirthDateUtc);
}