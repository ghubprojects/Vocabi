using DictionaryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryService.Infrastructure.EntityConfigurations;

internal sealed class DictionarySenseConfiguration : IEntityTypeConfiguration<DictionarySense>
{
    public void Configure(EntityTypeBuilder<DictionarySense> builder)
    {
        builder.ToTable("dictionary_senses", "dictionary");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.Definition)
            .IsRequired();

        builder.Property(x => x.OrderIndex)
            .IsRequired();

        builder.Property<Guid>("dictionary_entry_id");
    }
}
