using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabi.Domain.Aggregates.Pronunciations;

namespace Vocabi.Infrastructure.Persistence.EntityConfigurations;

public class PronunciationEntityTypeConfiguration : IEntityTypeConfiguration<Pronunciation>
{
    public void Configure(EntityTypeBuilder<Pronunciation> builder)
    {
        builder.ToTable("Pronunciations");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Word)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Ipa)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Word)
            .IsUnique();
    }
}