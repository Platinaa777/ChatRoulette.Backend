using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataContext.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("tokens");

        builder.HasKey(t => t.Id);
        builder.Property(id => id.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();
        
        builder.HasIndex(t => t.Token);

        builder.Property(t => t.Token)
            .HasColumnName("token")
            .HasConversion(
                to => to.Value,
                from => new Token(from));

        builder.Property(t => t.ExpiredAt)
            .HasColumnName("expired_at");

        builder.Property(t => t.IsUsed)
            .HasColumnName("is_used");
        
        builder.Property(t => t.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                to => to.Value,
                from => UserId.CreateId(from));
    }
}