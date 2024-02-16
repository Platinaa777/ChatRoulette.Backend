using AuthService.Domain.Models.UserAggregate.Entities;

namespace AuthService.Domain.Models.UserAggregate.Repos;

public interface IUserRepository
{
    Task<User?> FindUserByIdAsync(string id);
    Task<User?> FindUserByEmailAsync(string email);
    Task<bool> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
}