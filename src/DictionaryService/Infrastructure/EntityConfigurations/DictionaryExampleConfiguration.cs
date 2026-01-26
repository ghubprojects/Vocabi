using DictionaryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryService.Infrastructure.EntityConfigurations;

internal sealed class DictionaryExampleConfiguration : IEntityTypeConfiguration<DictionaryExample>
{
    public void Configure(EntityTypeBuilder<DictionaryExample> builder)
    {
        builder.ToTable("dictionary_examples", "dictionary");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.Text)
            .IsRequired();

        builder.Property<Guid>("dictionary_sense_id");
    }
}