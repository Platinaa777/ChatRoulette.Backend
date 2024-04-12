using Chat.Domain.Aggregates.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Chat.DataContext.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<TranslationGame>
{
    public void Configure(EntityTypeBuilder<TranslationGame> builder)
    {
        builder.ToTable("translation_games");
        
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.CorrectTranslation)
            .HasColumnName("correct_word");

        builder.Property(x => x.WordToTranslate)
            .HasColumnName("word");

        builder.Property(x => x.ChooseList)
            .HasColumnName("options")
            .HasConversion(
                x => JsonConvert.SerializeObject(x),
                val => JsonConvert.DeserializeObject <List<string>>(val)!);
    }
}