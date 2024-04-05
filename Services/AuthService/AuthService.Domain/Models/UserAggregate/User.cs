using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Events;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.UserAggregate;

public class User : AggregateRoot<Id>
{
    private User(
        Id id,
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
        RaiseDomainEvent(new CreateUserDomainEvent(Id.Value));
        NickName = nickname;
        Age = age;
    }
    
    public static Result<User> Create(
        string id, string userName,
        string email, string nickName, int age,
        string password, string salt, string? role)
    {
        Salt saltResult = new Salt(salt);
        RoleType roleType = RoleType.FromName(role) is null ? RoleType.UnactivatedUser : RoleType.FromName(role)!;

        var idResult = Id.CreateId(id);
        if (idResult.IsFailure)
            return Result.Failure<User>(idResult.Error);

        var userNameResult = Name.Create(userName);
        if (userNameResult.IsFailure)
            return Result.Failure<User>(userNameResult.Error);

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<User>(emailResult.Error);

        var nickNameResult = Name.Create(nickName);
        if (nickNameResult.IsFailure)
            return Result.Failure<User>(nickNameResult.Error);
        
        var ageResult = Age.Create(age);
        if (ageResult.IsFailure)
            return Result.Failure<User>(ageResult.Error);
        
        var passwordResult = Password.Create(password);
        if (passwordResult.IsFailure)
            return Result.Failure<User>(passwordResult.Error);
        return new User(
            idResult.Value,
            userNameResult.Value,
            emailResult.Value,
            nickNameResult.Value,
            ageResult.Value,
            passwordResult.Value,
            saltResult,
            roleType);
    }
    
    private User() : base() {}
}