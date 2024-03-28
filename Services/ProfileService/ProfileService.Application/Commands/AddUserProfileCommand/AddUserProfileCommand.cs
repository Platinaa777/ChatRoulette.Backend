using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.AddUserProfileCommand;

public class AddUserProfileCommand : IRequest<Result>
{
    public string Id { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

public static class AddUserProfileCommandToDomain
{
    public static Result<UserProfile> ToDomain(this AddUserProfileCommand command)
    {
        return UserProfile.Create(
            id: Guid.NewGuid().ToString(),
            command.NickName,
            command.Email,
            command.Age,
            rating: 0,
            friends: new List<string>()); // new account -> no friends and 0 rating
    }

    public static AddUserProfileCommand ToCommandFromMessage(this UserFullyRegistered req)
        => new AddUserProfileCommand()
        {
            Id = req.Id,
            Email = req.Email,
            NickName = req.NickName,
            Age = req.Age,
        };
}