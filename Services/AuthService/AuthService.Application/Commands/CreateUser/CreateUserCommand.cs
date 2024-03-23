using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Shared;
using MediatR;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public int Age { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public string[] Preferences { get; set; }
}

public static class CreateUserCommandToDomain
{
    public static User ToDomain(this CreateUserCommand command, IHasherPassword hasher)
    {
        var roleType = RoleType.FromName(command.Role);
        var salt = hasher.GenerateSalt();
        
        var hashedPassword = hasher.HashPasswordWithSalt(command.Password, salt);

        var userName = Name.Create(command.UserName);
        
        return new User(
            id: Guid.NewGuid().ToString(),
            Name.Create(command.UserName).Value,
            Email.Create(command.Email).Value,
            Name.Create(command.NickName).Value,
            Age.Create(command.Age).Value,
            Password.Create(command.Password).Value,
            new Salt(salt),
            roleType ?? RoleType.UnactivatedUser);
    }
}