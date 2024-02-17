using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Domain.SeedWork;

namespace ProfileService.Domain.Models.UserProfileAggregate.Entities;

public class UserProfile : Entity<string>
{
    public UserProfile(string id, Name userName, Name nickName, Email email)
    {
        Id = id;
        UserName = userName;
        NickName = nickName;
        Email = email;
    }
    
    public Name UserName { get; private set; }
    public Name NickName { get; private set; }
    public Email Email { get; private set; }
}