using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabi.Domain.Aggregates.LookupEntries;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Infrastructure.Persistence.EntityConfigurations;

public class LookupEntryEntityTypeConfiguration : IEntityTypeConfiguration<LookupEntry>
{
    public void Configure(EntityTypeBuilder<LookupEntry> builder)
    {
        builder.ToTable("LookupEntries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Headword)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.PartOfSpeech)
            .HasMaxLength(100);

        builder.Property(e => e.Pronunciation)
            .HasMaxLength(200);

        // Definitions
        builder.OwnsMany(e => e.Definitions, defBuilder =>
        {
            defBuilder.ToTable("LookupEntryDefinitions");

            defBuilder.WithOwner()
                .HasForeignKey(d => d.LookupEntryId);

            defBuilder.HasKey(d => d.Id);

            defBuilder.Property(d => d.Text)
                .IsRequired()
                .HasMaxLength(1000);

            // Examples
            defBuilder.OwnsMany(d => d.Examples, exBuilder =>
            {
                exBuilder.ToTable("LookupEntryExamples");

                exBuilder.WithOwner()
                    .HasForeignKey(e => e.LookupEntryDefinitionId);

                exBuilder.HasKey(e => e.Id);

                exBuilder.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(1000);
            });
        });

        // Meanings
        builder.OwnsMany(e => e.Meanings, mBuilder =>
        {
            mBuilder.ToTable("LookupEntryMeanings");

            mBuilder.WithOwner()
                .HasForeignKey(m => m.LookupEntryId);

            mBuilder.HasKey(m => m.Id);

            mBuilder.Property(m => m.Text)
                .IsRequired()
                .HasMaxLength(1000);
        });

        // Media files
        builder.OwnsMany(e => e.MediaFiles, fBuilder =>
        {
            fBuilder.ToTable("LookupEntryMediaFiles");

            fBuilder.WithOwner()
                .HasForeignKey(m => m.LookupEntryId);

            fBuilder.HasKey(m => m.Id);

            fBuilder.Property(m => m.MediaFileId)
                .IsRequired();

            fBuilder.HasOne<MediaFile>()
               .WithMany()
               .HasForeignKey(m => m.MediaFileId)
               .OnDelete(DeleteBehavior.Restrict);
        });
    }
}