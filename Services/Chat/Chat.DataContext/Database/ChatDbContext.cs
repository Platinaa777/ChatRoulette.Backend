using Chat.DataContext.Configurations;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.DataContext.Database;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    public DbSet<TwoSeatsRoom> Rooms { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ChatUserConfiguration());
        builder.ApplyConfiguration(new TwoSeatsRoomConfiguration());
        base.OnModelCreating(builder);
    }
}