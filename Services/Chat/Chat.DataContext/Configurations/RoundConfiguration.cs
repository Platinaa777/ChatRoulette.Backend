using Chat.Domain.Aggregates.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DataContext.Configurations;

public class RoundConfiguration : IEntityTypeConfiguration<Round>
{
    public void Configure(EntityTypeBuilder<Round> builder)
    {
        builder.ToTable("rounds");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.CorrectWord)
            .HasColumnName("correct_word");

        builder.Property(x => x.FirstPlayerAnswer)
            .HasColumnName("player1_answer");
        
        builder.Property(x => x.SecondPlayerAnswer)
            .HasColumnName("player2_answer");
        
        builder.Property(x => x.FirstPlayerAnswerTime)
            .HasColumnName("player1_answer_time");
        
        builder.Property(x => x.SecondPlayerAnswerTime)
            .HasColumnName("player2_answer_time");

        builder.Property(x => x.FirstPlayerEmail)
            .HasColumnName("player1_email");
        
        builder.Property(x => x.SecondPlayerEmail)
            .HasColumnName("player2_email");
    }
}