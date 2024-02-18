using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Application.Commands;

public class AddUserProfileCommand : IRequest<bool>
{
    public string UserName { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
}

public static class AddUserProfileCommandToDomain
{
    public static UserProfile ToDomain(this AddUserProfileCommand command)
    {
        return new UserProfile(
            id: Guid.NewGuid().ToString(),
            userName: new Name(command.UserName),
            nickName: new Name(command.NickName),
            new Email(command.Email));
    }
}