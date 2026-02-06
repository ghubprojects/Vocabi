using DictionaryService.Domain.Aggregates.DictionaryEntries.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryService.Infrastructure.Persistence.EntityConfigurations;

internal sealed class DictionaryDefinitionConfiguration : IEntityTypeConfiguration<DictionaryDefinition>
{
    public void Configure(EntityTypeBuilder<DictionaryDefinition> builder)
    {
        builder.ToTable("DictionaryDefinition");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.OrderIndex)
            .IsRequired();

        builder.Property<Guid>("DictionaryEntryId");
    }
}
