using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataContext.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUser(builder);
    }

    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        // for id
        builder.HasKey(u => u.Id);
        builder.Property(id => id.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                val => val.Value,
                value => Id.CreateId(value).Value);

        // for name
        builder.Property(n => n.UserName)
            .HasColumnName("username")
            .HasConversion(
                name => name.Value, // to database
                value => Name.Create(value).Value // from database
            );

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasConversion(
                email => email.Value, // to database
                value => Email.Create(value).Value // from database
            );;

        builder.Property(u => u.IsSubmittedEmail)
            .HasColumnName("confirmation");

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Ignore(u => u.BirthDateUtc);
        
        builder.Property(n => n.PasswordHash)
            .HasColumnName("password")
            .HasConversion(
                name => name.Value, // to database
                value => Password.Create(value).Value // from database
            );
        
        builder.Property(n => n.Salt)
            .HasColumnName("salt")
            .HasConversion(
                salt => salt.Value, // to database
                value => new Salt(value) // from database
            );

        builder.Property<RoleType>(r => r.Role)
            .HasColumnName("role")
            .HasConversion(
                role => role.Name,
                roleName => RoleType.FromName(roleName)!
            );
    }
}