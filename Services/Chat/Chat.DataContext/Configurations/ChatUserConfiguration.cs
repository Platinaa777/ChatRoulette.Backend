using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Chat.DataContext.Configurations;

public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
{
    public void Configure(EntityTypeBuilder<ChatUser> builder)
    {
        builder.ToTable("chat_users");
    
        // for id
        builder.HasKey(u => u.Id);
        builder.Property(id => id.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(u => u.ConnectionId)
            .HasColumnName("connection_id");
    
        // for name
        builder.Property(n => n.Email)
            .HasColumnName("email");
    
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PreviousParticipantEmails)
            .HasColumnName("peers_list")
            .HasConversion(
                val => 
                    JsonConvert.SerializeObject(val, Formatting.Indented),
                val =>
                    JsonConvert.DeserializeObject<HashSet<string>>(val) ?? new HashSet<string>());
    }
}