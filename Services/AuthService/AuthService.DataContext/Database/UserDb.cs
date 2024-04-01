using AuthService.DataContext.Configurations;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataContext.Database;

public class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(builder);
    }
}
