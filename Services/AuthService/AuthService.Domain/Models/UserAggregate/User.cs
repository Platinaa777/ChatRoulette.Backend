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
        BirthDateUtc birthDateUtc,
        Password passwordHash,
        Salt salt,
        RoleType role) : base(id)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        BirthDateUtc = birthDateUtc;
        Salt = salt;
        Role = role;
    }
    
    public Name UserName { get; private set; }
    
    public Email Email { get; private set; }
    public bool IsSubmittedEmail { get; private set; }
    
    public BirthDateUtc BirthDateUtc { get; private set; }
    
    public Password PasswordHash { get; private set; }
    
    public Salt Salt { get; private set; }

    public RoleType Role { get; private set; }

    public void SubmitEmail()
    {
        IsSubmittedEmail = true;
    }

    public void Register(BirthDateUtc birthDateUtc)
    {
        RaiseDomainEvent(new CreateUserDomainEvent(Id.Value));
        BirthDateUtc = birthDateUtc;
    }
    
    public static Result<User> Create(
        string id, string userName,
        string email, DateTime birthDateUtc,
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

        var birthDateUtcResult = BirthDateUtc.Create(birthDateUtc);
        if (birthDateUtcResult.IsFailure)
            return Result.Failure<User>(birthDateUtcResult.Error);
        
        var passwordResult = Password.Create(password);
        if (passwordResult.IsFailure)
            return Result.Failure<User>(passwordResult.Error);
        return new User(
            idResult.Value,
            userNameResult.Value,
            emailResult.Value,
            birthDateUtcResult.Value,
            passwordResult.Value,
            saltResult,
            roleType);
    }
    
    private User() : base() {}
}