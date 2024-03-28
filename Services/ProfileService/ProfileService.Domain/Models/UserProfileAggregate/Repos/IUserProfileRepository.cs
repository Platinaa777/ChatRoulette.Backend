namespace ProfileService.Domain.Models.UserProfileAggregate.Repos;

public interface IUserProfileRepository
{
    Task<UserProfile?> FindUserByIdAsync(string id);
    Task<UserProfile?> FindUserByEmailAsync(string email);
    Task<bool> AddUserAsync(UserProfile user);
    Task<bool> UpdateUserAsync(UserProfile user);
    Task<List<UserProfile>> GetAllUsers();
}