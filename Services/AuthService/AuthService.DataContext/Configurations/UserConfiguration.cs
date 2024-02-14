using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataContext.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private IEntityTypeConfiguration<User> _entityTypeConfigurationImplementation;
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUser(builder);
    }

    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        //todo conversations
        builder.ToTable("Users");

        // for id
        builder.HasKey(u => u.Id);
        builder.Property(id => id.Id)
            .ValueGeneratedNever();

        // for name
        builder.Property(n => n.UserName)
            .HasConversion(
                name => name.Value, // to database
                value => new Name(value) // from database
            );

        builder.OwnsOne<Email>(e => e.Email);
        
        builder.Property(n => n.NickName)
            .HasConversion(
                name => name.Value, // to database
                value => new Name(value) // from database
            );
        
        builder.Property(n => n.Age)
            .HasConversion(
                name => name.Value, // to database
                value => new Age(value) // from database
            );

        
        builder.Property(n => n.PasswordHash)
            .HasConversion(
                name => name.Value, // to database
                value => new Password(value) // from database
            );

        builder.OwnsOne<RoleType>(r => r.Role);
    }
}