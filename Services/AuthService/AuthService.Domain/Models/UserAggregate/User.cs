using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Shared;

namespace AuthService.Domain.Models.UserAggregate;

public class User : Entity<string>
{
    
    public User(
        string id,
        Name userName,
        Email email,
        Name nickName,
        Age age,
        Password passwordHash,
        Salt salt,
        RoleType role) : base(id)
    {
        UserName = userName;
        Email = email;
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

    public void AddUserExtraInformation(Name nickname, Age age)
    {
        NickName = nickname;
        Age = age;
    }
    
    private User() : base() {}
}