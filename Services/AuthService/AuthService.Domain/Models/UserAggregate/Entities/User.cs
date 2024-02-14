using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.Entities;

public class User : Entity<string>
{
    public User()
    {
        
    }
    
    public User(string id, Name userName, Email email, Name nickName, Age age, Password passwordHash, RoleType role)
    {
        Id = id;
        UserName = userName;
        Email = email;
        NickName = nickName;
        Age = age;
        PasswordHash = passwordHash;
        Role = role;
    }
    
    public Name UserName { get; private set; }
    
    public Email Email { get; private set; }
    
    public Name NickName { get; private set; }
    
    public Age Age { get; private set; }
    
    public Password PasswordHash { get; private set; }

    // todo chanage to role 
    public RoleType Role { get; set; }
}