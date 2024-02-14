using AuthService.DataContext.Database;
using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Repos;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repos;

public class UserRepository : IUserRepository
{
    private readonly UserDb _context;

    public UserRepository(UserDb context)
    {
        _context = context;
    }
    
    public Task<User> FindUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> FindUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddUserAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (existingUser != null)
            return false;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public Task<bool> UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}