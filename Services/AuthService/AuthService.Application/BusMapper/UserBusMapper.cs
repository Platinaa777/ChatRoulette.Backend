using AuthService.Domain.Models.UserAggregate;
using MassTransit.Contracts.UserEvents;

namespace AuthService.Application.BusMapper;

public static class UserBusMapper
{
    public static UserRegistered ToBusMessage(this User user)
        => new()
        {
            Id = user.Id.Value,
            Email = user.Email.Value,
            UserName = user.UserName.Value,
        };
}