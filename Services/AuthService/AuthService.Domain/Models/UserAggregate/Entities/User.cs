using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.Entities;

public class User : Entity<string>
{
    
    public User(string id, Name userName, Email email, Name nickName, Age age, Password passwordHash, Salt salt, RoleType role)
    {
        Id = id;
        UserName = userName;
        Email = email;
        NickName = nickName;
        Age = age;
        PasswordHash = passwordHash;
        Salt = salt;
        Role = role;
    }
    
    public Name UserName { get; private set; }
    
    public Email Email { get; private set; }
    public bool IsSubmittedEmail { get; private set; }
    
    public Name NickName { get; private set; }
    
    public Age Age { get; private set; }
    
    public Password PasswordHash { get; private set; }
    
    public Salt Salt { get; private set; }

    public RoleType Role { get; private set; }

    public void SubmitEmail()
    {
        IsSubmittedEmail = true;
    }
    
    private User() { }
}