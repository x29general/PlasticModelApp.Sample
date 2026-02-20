using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.Data.Configurations;

public sealed class PaintEntityConfiguration : IEntityTypeConfiguration<PaintEntity>
{
    public void Configure(EntityTypeBuilder<PaintEntity> builder)
    {
        builder.ToTable("paints");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ModelNumber)
            .HasColumnName("model_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ModelNumberPrefix)
            .HasColumnName("model_number_prefix")
            .HasMaxLength(50);

        builder.Property(x => x.ModelNumberNumber)
            .HasColumnName("model_number_number");

        builder.Property(x => x.BrandId)
            .HasColumnName("brand_id")
            .IsRequired();

        builder.Property(x => x.PaintTypeId)
            .HasColumnName("paint_type_id")
            .IsRequired();

        builder.Property(x => x.GlossId)
            .HasColumnName("gloss_id")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(10,2)")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(x => x.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(200);

        builder.Property(x => x.Hex)
            .HasColumnName("hex")
            .IsRequired();

        builder.Property(x => x.RgbR)
            .HasColumnName("rgb_r")
            .IsRequired();

        builder.Property(x => x.RgbG)
            .HasColumnName("rgb_g")
            .IsRequired();

        builder.Property(x => x.RgbB)
            .HasColumnName("rgb_b")
            .IsRequired();

        builder.Property(x => x.HslH)
            .HasColumnName("hsl_h")
            .IsRequired();

        builder.Property(x => x.HslS)
            .HasColumnName("hsl_s")
            .IsRequired();

        builder.Property(x => x.HslL)
            .HasColumnName("hsl_l")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(x => x.Brand)
            .WithMany(x => x.Paints)
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(x => x.PaintType)
            .WithMany(x => x.Paints)
            .HasForeignKey(x => x.PaintTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(x => x.Gloss)
            .WithMany(x => x.Paints)
            .HasForeignKey(x => x.GlossId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(x => x.PaintTags)
            .WithOne(x => x.Paint)
            .HasForeignKey(x => x.PaintId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.BrandId);
        builder.HasIndex(x => x.PaintTypeId);
        builder.HasIndex(x => x.GlossId);
        builder.HasIndex(x => new { x.BrandId, x.ModelNumber }).IsUnique();
        builder.HasIndex(x => new { x.ModelNumberPrefix, x.ModelNumberNumber });
    }
}
