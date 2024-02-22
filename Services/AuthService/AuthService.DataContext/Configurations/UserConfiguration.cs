using AuthService.Domain.Models.UserAggregate.Entities;
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
            .ValueGeneratedNever();

        // for name
        builder.Property(n => n.UserName)
            .HasColumnName("username")
            .HasConversion(
                name => name.Value, // to database
                value => new Name(value) // from database
            );

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasConversion(
                email => email.Value, // to database
                value => new Email(value) // from database
            );;

        builder.Property(u => u.IsSubmittedEmail)
            .HasColumnName("confirmation");

        builder.HasIndex(u => u.Email);

        builder.Ignore(u => u.Age);
        builder.Ignore(u => u.NickName);
        
        // builder.Property(n => n.NickName)
        //     .HasColumnName("nickname")
        //     .HasConversion(
        //         name => name.Value, // to database
        //         value => new Name(value) // from database
        //     );
        //
        // builder.Property(n => n.Age)
        //     .HasColumnName("age")
        //     .HasConversion(
        //         name => name.Value, // to database
        //         value => new Age(value) // from database
        //     );

        
        builder.Property(n => n.PasswordHash)
            .HasColumnName("password")
            .HasConversion(
                name => name.Value, // to database
                value => new Password(value) // from database
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