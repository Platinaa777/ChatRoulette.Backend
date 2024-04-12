using Chat.DataContext.Configurations;
using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Game;
using Chat.Domain.Aggregates.Room;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataContext.Database;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    public DbSet<TwoSeatsRoom> Rooms { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<TranslationGame> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ChatUserConfiguration());
        builder.ApplyConfiguration(new TwoSeatsRoomConfiguration());
        builder.ApplyConfiguration(new GameConfiguration());
        builder.ApplyConfiguration(new RoundConfiguration());
        base.OnModelCreating(builder);
    }
}