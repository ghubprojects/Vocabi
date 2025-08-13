using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabi.Domain.Aggregates.MediaFiles;

namespace Vocabi.Infrastructure.Persistence.EntityConfigurations;

public class MediaFileEntityTypeConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.ToTable("MediaFiles");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.FilePath)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(m => m.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Size)
            .IsRequired();

        builder.Property(m => m.SourceCategory)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(m => m.SourceName)
            .HasMaxLength(255);
    }
}