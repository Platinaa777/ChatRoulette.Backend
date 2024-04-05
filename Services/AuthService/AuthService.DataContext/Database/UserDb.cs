using AuthService.DataContext.Configurations;
using AuthService.DataContext.Outbox;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserHistoryAggregate;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataContext.Database;

public class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options) : base(options)
    {
    }
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.ApplyConfiguration(new HistoryConfiguration());
        base.OnModelCreating(builder);
    }
}
