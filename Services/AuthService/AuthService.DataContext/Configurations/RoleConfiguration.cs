using AuthService.Domain.Models.UserAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataContext.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .HasColumnName("RoleId");

        builder.Property(r => r.Value)
            .HasColumnName("RoleValue");
    }
}