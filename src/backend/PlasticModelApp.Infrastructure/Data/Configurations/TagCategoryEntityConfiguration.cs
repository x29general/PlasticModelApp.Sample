using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.Data.Configurations;

public sealed class TagCategoryEntityConfiguration : IEntityTypeConfiguration<TagCategoryEntity>
{
    public void Configure(EntityTypeBuilder<TagCategoryEntity> builder)
    {
        builder.ToTable("tag_categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description");
    }
}
