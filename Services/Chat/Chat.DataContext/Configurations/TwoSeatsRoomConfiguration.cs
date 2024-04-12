using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;
using Chat.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Chat.DataContext.Configurations;

public class TwoSeatsRoomConfiguration : IEntityTypeConfiguration<TwoSeatsRoom>
{
    public void Configure(EntityTypeBuilder<TwoSeatsRoom> builder)
    {
        builder.ToTable("rooms");
        
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(r => r.ClosedAt)
            .HasColumnName("closed_at");

        builder.Property(x => x.PeerLinks)
            .HasColumnName("peers_emails")
            .HasConversion(
                val => 
                    JsonConvert.SerializeObject(val, Formatting.Indented),
                val =>
                    JsonConvert.DeserializeObject<List<UserLink>>(val) ?? new List<UserLink>());
    }
}