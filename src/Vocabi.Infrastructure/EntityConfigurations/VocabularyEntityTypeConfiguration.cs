using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.EntityConfigurations;

public class VocabularyEntityTypeConfiguration : IEntityTypeConfiguration<Vocabulary>
{
    public void Configure(EntityTypeBuilder<Vocabulary> builder)
    {
        builder.ToTable("Vocabularies");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Word)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(v => v.PartOfSpeech)
            .HasMaxLength(100);

        builder.Property(v => v.Pronunciation)
            .HasMaxLength(200);

        builder.Property(v => v.Cloze)
            .HasMaxLength(500);

        builder.Property(v => v.Definition)
            .HasMaxLength(1000);

        builder.Property(v => v.Example)
            .HasMaxLength(1000);

        builder.Property(v => v.Meaning)
            .HasMaxLength(1000);

        builder.Property(v => v.IsSyncedToAnki)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        // Media files
        builder.OwnsMany(e => e.MediaFiles, fBuilder =>
        {
            fBuilder.ToTable("VocabularyMediaFiles");

            fBuilder.HasKey(m => m.Id);

            fBuilder.WithOwner()
                .HasForeignKey(m => m.VocabularyId);

            fBuilder.Property(m => m.MediaFileId)
                .IsRequired();

            fBuilder.HasOne<MediaFile>()
               .WithMany()
               .HasForeignKey(m => m.MediaFileId)
               .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
