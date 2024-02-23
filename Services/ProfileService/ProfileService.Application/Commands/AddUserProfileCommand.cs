using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Application.Commands;

public class AddUserProfileCommand : IRequest<bool>
{
    public string NickName { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string[] Preference { get; set; }
}

public static class AddUserProfileCommandToDomain
{
    public static UserProfile ToDomain(this AddUserProfileCommand command)
    {
        List<Preference> preferences = new(); 
        foreach (var preference in command.Preference)
        {
            var p = Preference.FromName(preference);
            if (p != null)
            {
                preferences.Add(p);
            }
        }
        
        return new UserProfile(
            id: Guid.NewGuid().ToString(),
            nickName: new Name(command.NickName),
            new Email(command.Email),
            new Age(command.Age),
            preferences.ToArray());
    }

    public static AddUserProfileCommand ToCommandFromMessage(this UserFullyRegistered req)
        => new AddUserProfileCommand()
        {
            Email = req.Email,
            NickName = req.NickName,
            Age = req.Age,
            Preference = req.Preferences
        };
}