using DictionaryService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryService.Infrastructure.Persistence.EntityConfigurations;

internal sealed class DictionaryEntryConfiguration : IEntityTypeConfiguration<DictionaryEntry>
{
    public void Configure(EntityTypeBuilder<DictionaryEntry> builder)
    {
        builder.ToTable("DictionaryEntry");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Headword)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PartOfSpeech)
            .HasMaxLength(50);

        builder.Property(x => x.Pronunciation)
            .HasMaxLength(100);

        builder.Property(x => x.Source)
            .HasMaxLength(200);
    }
}