using AuthService.Domain.Models.UserAggregate.Entities;
using MassTransit.Contracts.UserEvents;

namespace AuthService.Application.BusMapper;

public static class UserBusMapper
{
    public static UserRegistered ToBusMessage(this User user)
        => new UserRegistered()
        {
            Email = user.Email.Value,
            UserName = user.UserName.Value,
            NickName = user.NickName.Value
        };
}