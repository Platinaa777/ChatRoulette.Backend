using AdminService.Domain.Models.ComplaintAggregate.Enumerations;
using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;
using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.Domain.Models.FeedbackAggregate.ValueObjects;
using AdminService.Domain.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminService.DataContext.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        ConfigureFeedback(builder);
    }

    private void ConfigureFeedback(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("feedbacks");

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
                value => FeedbackContent.Create(value).Value // from database
            );

        builder.Property(u => u.EmailFrom)
            .HasColumnName("email_from")
            .HasConversion(
                email => email.Value, // to database
                value => Email.Create(value).Value // from database
            );

        builder.Property(x => x.IsWatched)
            .HasColumnName("is_watched");
    }
}