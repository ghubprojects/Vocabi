using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabi.Domain.Aggregates.MediaFiles;
using Vocabi.Domain.Aggregates.Vocabularies;

namespace Vocabi.Infrastructure.Persistence.EntityConfigurations;

public class VocabularyEntityTypeConfiguration : IEntityTypeConfiguration<Vocabulary>
{
    public void Configure(EntityTypeBuilder<Vocabulary> builder)
    {
        builder.ToTable("Vocabularies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Word)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PartOfSpeech)
            .HasMaxLength(50);

        builder.Property(x => x.Pronunciation)
            .HasMaxLength(200);

        builder.Property(x => x.Cloze)
            .HasMaxLength(500);

        builder.Property(x => x.Definition)
            .HasMaxLength(1000);

        builder.Property(x => x.Example)
            .HasMaxLength(1000);

        builder.Property(x => x.Meaning)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Media files
        builder.OwnsMany(x => x.MediaFiles, b =>
        {
            b.ToTable("VocabularyMediaFiles");

            b.HasKey(m => m.Id);

            b.WithOwner()
            .HasForeignKey(m => m.VocabularyId);

            b.Property(m => m.MediaFileId)
            .IsRequired();

            b.HasOne<MediaFile>()
            .WithMany()
            .HasForeignKey(m => m.MediaFileId)
            .OnDelete(DeleteBehavior.Restrict);
        });

        // Flashcards
        builder.OwnsOne(x => x.Flashcard, b =>
        {
            b.Property(f => f.NoteId);

            b.Property(f => f.Status)
             .HasConversion<string>()
             .IsRequired();

            b.Property(f => f.CreatedAt)
            .IsRequired();

            b.Property(f => f.ExportedAt);

            b.Property(f => f.LastTriedAt);
        });
    }
}