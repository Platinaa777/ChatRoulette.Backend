using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserHistoryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataContext.Configurations;

public class HistoryConfiguration : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        ConfigureHistory(builder);
    }

    private void ConfigureHistory(EntityTypeBuilder<History> builder)
    {
        builder.ToTable("histories");
        
        builder.HasKey(t => t.Id);
        builder.Property(id => id.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                val => val.Value.ToString(),
                value => Id.CreateId(value).Value);
        
        builder.Property(id => id.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedNever()
            .HasConversion(
                val => val.Value.ToString(),
                value => Id.CreateId(value).Value);
        
        builder.HasIndex(t => t.UserId)
            .IsUnique();

        builder.Property(x => x.BannedTime)
            .HasColumnName("banned_time");
    }
}