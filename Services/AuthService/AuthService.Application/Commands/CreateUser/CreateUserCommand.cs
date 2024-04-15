using AuthService.Application.Security;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result>
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDateUtc { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public static class CreateUserCommandToDomain
{
    public static Result<User> ToDomain(this CreateUserCommand command, IHasherPassword hasher)
    {
        var salt = hasher.GenerateSalt();
        
        var hashedPassword = hasher.HashPasswordWithSalt(command.Password, salt);

        return User.Create(
            id: Guid.NewGuid().ToString(),
            command.UserName,
            command.Email,
            command.BirthDateUtc,
            hashedPassword,
            salt,
            command.Role);
    }
}