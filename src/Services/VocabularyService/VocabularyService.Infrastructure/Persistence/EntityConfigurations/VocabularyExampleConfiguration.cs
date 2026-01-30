using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VocabularyService.Domain.Aggregates;

namespace VocabularyService.Infrastructure.Persistence.EntityConfigurations;

internal sealed class VocabularyExampleConfiguration : IEntityTypeConfiguration<VocabularyExample>
{
    public void Configure(EntityTypeBuilder<VocabularyExample> builder)
    {
        builder.ToTable("VocabularyExample");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(2000);

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
