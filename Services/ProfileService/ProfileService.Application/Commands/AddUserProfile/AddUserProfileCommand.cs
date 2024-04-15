using DomainDriverDesignAbstractions;
using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate;

namespace ProfileService.Application.Commands.AddUserProfile;

public class AddUserProfileCommand : IRequest<Result>
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDateUtc { get; set; }
}

public static class AddUserProfileCommandToDomain
{
    public static Result<UserProfile> ToDomain(this AddUserProfileCommand command)
    {
        return UserProfile.Create(
            id: command.Id,
            command.UserName,
            command.Email,
            command.BirthDateUtc,
            // new account
            rating: 0,  //  0 rating
            friends: new List<string>(), // no friends
            jsonAchievements: string.Empty,
            ""); // no achievements
    }

    public static AddUserProfileCommand ToCommandFromMessage(this UserFullyRegistered req)
        => new()
        {
            Id = req.Id,
            Email = req.Email,
            UserName = req.UserName,
            BirthDateUtc = req.BirthDateUtc,
        };
}