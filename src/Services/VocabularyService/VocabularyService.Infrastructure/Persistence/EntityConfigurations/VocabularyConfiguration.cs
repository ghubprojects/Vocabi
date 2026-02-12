using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Infrastructure.Persistence.EntityConfigurations;

internal sealed class VocabularyConfiguration : IEntityTypeConfiguration<Vocabulary>
{
    public void Configure(EntityTypeBuilder<Vocabulary> builder)
    {
        builder.ToTable("Vocabulary");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Headword)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PartOfSpeech)
            .HasMaxLength(50);

        builder.Property(x => x.Pronunciation)
            .HasMaxLength(100);

        builder.Property(x => x.Cloze)
            .HasMaxLength(200);

        builder.Property(x => x.Definition)
            .HasMaxLength(2000);

        builder.Property(x => x.Translation)
            .HasMaxLength(1000);

        builder.OwnsOne(x => x.Audit, audit =>
        {
            audit.Property(a => a.CreatedAt)
                .IsRequired();

            audit.Property(a => a.CreatedBy);

            audit.Property(a => a.LastModifiedAt);

            audit.Property(a => a.LastModifiedBy);
        });

        builder.OwnsOne(x => x.SoftDelete, softDelete =>
        {
            softDelete.Property(s => s.IsDeleted)
                .IsRequired();

            softDelete.Property(s => s.DeletedAt);

            softDelete.Property(s => s.DeletedBy);
        });
    }
}