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
        NickName = nickName;
        Age = age;
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
    
    public static Result<User> Create(
        Guid id, string userName,
        string email, string nickName, int age,
        string password, string salt, string? role)
    {
        Salt saltResult = new Salt(salt);
        RoleType roleType = RoleType.FromName(role) is null ? RoleType.UnactivatedUser : RoleType.FromName(role)!;

        return Start.From<User>().Then<User, Name>()
            .Check(Name.Create, userName, out var userNameValue).Then<Name, Email>()
            .Check(Email.Create, email, out var emailValue).Then<Email, Name>()
            .Check(Name.Create, nickName, out var nickNameValue).Then<Name, Age>()
            .Check(Age.Create, age, out var ageValue).Then<Age, Password>()
            .Check(Password.Create, password, out var passwordValue)
            .GetResult(new User(
                id.ToString(),
                userNameValue,
                emailValue,
                nickNameValue,
                ageValue,
                passwordValue,
                saltResult,
                roleType));
    }
    
    private User() : base() {}
}