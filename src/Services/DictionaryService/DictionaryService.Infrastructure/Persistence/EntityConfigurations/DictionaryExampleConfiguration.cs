using DictionaryService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryService.Infrastructure.Persistence.EntityConfigurations;

internal sealed class DictionaryExampleConfiguration : IEntityTypeConfiguration<DictionaryExample>
{
    public void Configure(EntityTypeBuilder<DictionaryExample> builder)
    {
        builder.ToTable("DictionaryExample");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property<Guid>("DictionarySenseId");
    }
}