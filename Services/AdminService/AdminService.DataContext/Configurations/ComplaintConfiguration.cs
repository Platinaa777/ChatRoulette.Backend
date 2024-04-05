using AdminService.Domain.Models.ComplaintAggregate;
using AdminService.Domain.Models.ComplaintAggregate.Enumerations;
using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;
using AdminService.Domain.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminService.DataContext.Configurations;

public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        ConfigureComplaint(builder);
    }

    private void ConfigureComplaint(EntityTypeBuilder<Complaint> builder)
    {
        builder.ToTable("complaints");

        // for id
        builder.HasKey(u => u.Id);
        builder.Property(id => id.Id)
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                name => name.Value, // to database
                value => Id.Create(value.ToString()).Value // from database
            );;

        // for name
        builder.Property(n => n.Content)
            .HasColumnName("content")
            .HasConversion(
                name => name.Value, // to database
                value => Content.Create(value).Value // from database
            );

        builder.Property(u => u.SenderEmail)
            .HasColumnName("sender_email")
            .HasConversion(
                email => email.Value, // to database
                value => Email.Create(value).Value // from database
            );;
        
        builder.Property(n => n.ViolatorEmail)
            .HasColumnName("violator_email")
            .HasConversion(
                name => name.Value, // to database
                value => Email.Create(value).Value // from database
            );
        
        builder.Property<ComplaintType>(r => r.ComplaintType)
            .HasColumnName("type")
            .HasConversion(
                role => role.Name,
                roleName => ComplaintType.FromName(roleName)!
            );

        builder.Property(n => n.IsHandled)
            .HasColumnName("is_handled");
    }
}