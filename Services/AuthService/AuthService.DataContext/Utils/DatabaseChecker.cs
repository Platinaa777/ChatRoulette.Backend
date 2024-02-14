using AuthService.DataContext.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataContext.Utils;

public static class DatabaseChecker
{
    public static async Task<bool> IsDbEmpty(UserDb context)
    {
        return await context.Users.FirstOrDefaultAsync(u => true) == null ? false : true;
    }
}