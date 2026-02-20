using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.Data.Configurations;

public sealed class TagEntityConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.ToTable("tags");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TagCategoryId)
            .HasColumnName("tag_category_id")
            .IsRequired();

        builder.Property(x => x.Hex)
            .HasColumnName("hex")
            .HasMaxLength(7);

        builder.Property(x => x.Effect)
            .HasColumnName("effect");

        builder.Property(x => x.Description)
            .HasColumnName("description");

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

        builder.HasOne(x => x.TagCategory)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.TagCategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasMany(x => x.PaintTags)
            .WithOne(x => x.Tag)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TagCategoryId);
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
