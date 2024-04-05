using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserAggregate.ValueObjects;

namespace AuthService.Domain.Models.UserAggregate.Repos;

public interface IUserRepository
{
    Task<User?> FindUserByIdAsync(Id id);
    Task<User?> FindUserByEmailAsync(Email email);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
}