using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects.User;
using MediatR;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<bool>
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
        
        return new User(
            id: Guid.NewGuid().ToString(),
            new Name(command.UserName),
            new Email(command.Email),
            new Name(command.NickName),
            new Age(command.Age),
            new Password(hashedPassword),
            new Salt(salt),
            roleType ?? RoleType.UnactivatedUser);
    }
}