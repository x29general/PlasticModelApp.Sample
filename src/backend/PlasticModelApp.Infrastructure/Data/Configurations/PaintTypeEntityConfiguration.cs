using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.Data.Configurations;

public sealed class PaintTypeEntityConfiguration : IEntityTypeConfiguration<PaintTypeEntity>
{
    public void Configure(EntityTypeBuilder<PaintTypeEntity> builder)
    {
        builder.ToTable("paint_types");

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
