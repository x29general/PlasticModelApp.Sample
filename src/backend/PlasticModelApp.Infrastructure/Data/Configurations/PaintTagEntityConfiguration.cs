using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.Data.Configurations;

public sealed class PaintTagEntityConfiguration : IEntityTypeConfiguration<PaintTagEntity>
{
    public void Configure(EntityTypeBuilder<PaintTagEntity> builder)
    {
        builder.ToTable("paint_tags");

        builder.HasKey(x => new { x.PaintId, x.TagId });

        builder.Property(x => x.PaintId)
            .HasColumnName("paint_id")
            .IsRequired();

        builder.Property(x => x.TagId)
            .HasColumnName("tag_id")
            .IsRequired();

        builder.HasOne(x => x.Paint)
            .WithMany(x => x.PaintTags)
            .HasForeignKey(x => x.PaintId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(x => x.Tag)
            .WithMany(x => x.PaintTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasIndex(x => x.PaintId);
        builder.HasIndex(x => x.TagId);
    }
}
