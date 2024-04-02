using DomainDriverDesignAbstractions;
using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate;

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
            id: command.Id,
            command.NickName,
            command.Email,
            command.Age,
            // new account
            rating: 0,  //  0 rating
            friends: new List<string>(), // no friends
            jsonAchievements: string.Empty,
            ""); // no achievements
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