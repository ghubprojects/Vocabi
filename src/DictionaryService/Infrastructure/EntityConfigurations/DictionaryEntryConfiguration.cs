using DictionaryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryService.Infrastructure.EntityConfigurations;

internal sealed class DictionaryEntryConfiguration : IEntityTypeConfiguration<DictionaryEntry>
{
    public void Configure(EntityTypeBuilder<DictionaryEntry> builder)
    {
        builder.ToTable("dictionary_entries", "dictionary");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Headword)
            .IsRequired();

        builder.Property(x => x.PartOfSpeech);

        builder.Property(x => x.Pronunciation);

        builder.Property(x => x.Source);
    }
}