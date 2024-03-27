using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Domain.Shared;
using Preference = ProfileService.Domain.Models.UserProfileAggregate.Enumerations.Preference;

namespace ProfileService.Domain.Models.UserProfileAggregate.Entities;

public class UserProfile : Entity<string>
{

    public UserProfile(
        string id,
        Name nickName,
        Email email,
        Age age,
        Preference[] preferences)
    {
        Id = id;
        NickName = nickName;
        Email = email;
        Age = age;
        Preferences = preferences;
    }

    public void SetNickName(string name)
    {
        var newName = new Name(name);

        NickName = newName;
    }

    public void SetEmail(string email)
    {
        var newEmail = new Email(email);

        Email = newEmail;
    }
    
    public Name NickName { get; private set; }
    public Email Email { get; private set; }
    public Age Age { get; private set; }
    public Preference[] Preferences { get; private set; }
}