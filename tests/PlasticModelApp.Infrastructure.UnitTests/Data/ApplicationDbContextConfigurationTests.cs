using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Infrastructure.Data;
using PlasticModelApp.Infrastructure.Data.Entities;
using PlasticModelApp.Infrastructure.UnitTests.Fixtures;

namespace PlasticModelApp.Infrastructure.UnitTests.Data;

public sealed class ApplicationDbContextConfigurationTests
{
    [Fact]
    public void PaintEntity_Should_Map_To_Paints_Table()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();

        var entityType = context.Model.FindEntityType(typeof(PaintEntity));

        entityType.Should().NotBeNull();
        entityType!.GetTableName().Should().Be("paints");
    }

    [Fact]
    public void PaintTagEntity_Should_Have_Composite_PrimaryKey()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();

        var entityType = context.Model.FindEntityType(typeof(PaintTagEntity));

        entityType.Should().NotBeNull();
        var keyProperties = entityType!.FindPrimaryKey()!.Properties.Select(p => p.Name);
        keyProperties.Should().Equal(nameof(PaintTagEntity.PaintId), nameof(PaintTagEntity.TagId));
    }

    [Fact]
    public void PaintEntity_Should_Have_Unique_And_Composite_Indexes()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();

        var entityType = context.Model.FindEntityType(typeof(PaintEntity));
        entityType.Should().NotBeNull();

        var indexes = entityType!.GetIndexes().ToList();

        indexes.Should().Contain(i =>
            i.IsUnique &&
            i.Properties.Select(p => p.Name).SequenceEqual(new[] { nameof(PaintEntity.BrandId), nameof(PaintEntity.ModelNumber) }));

        indexes.Should().Contain(i =>
            !i.IsUnique &&
            i.Properties.Select(p => p.Name).SequenceEqual(new[] { nameof(PaintEntity.ModelNumberPrefix), nameof(PaintEntity.ModelNumberNumber) }));
    }

    [Fact]
    public void QueryFilter_Should_Exist_Only_On_Paint_And_Tag()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();

        context.Model.FindEntityType(typeof(PaintEntity))!.GetDeclaredQueryFilters().Should().NotBeEmpty();
        context.Model.FindEntityType(typeof(TagEntity))!.GetDeclaredQueryFilters().Should().NotBeEmpty();

        context.Model.FindEntityType(typeof(BrandEntity))!.GetDeclaredQueryFilters().Should().BeEmpty();
        context.Model.FindEntityType(typeof(PaintTypeEntity))!.GetDeclaredQueryFilters().Should().BeEmpty();
        context.Model.FindEntityType(typeof(GlossEntity))!.GetDeclaredQueryFilters().Should().BeEmpty();
        context.Model.FindEntityType(typeof(TagCategoryEntity))!.GetDeclaredQueryFilters().Should().BeEmpty();
    }

    [Fact]
    public void OnModelCreating_Should_Set_DefaultSchema_For_NpgsqlProvider()
    {
        var oldSchema = Environment.GetEnvironmentVariable("DATABASE_SCHEMA");
        Environment.SetEnvironmentVariable("DATABASE_SCHEMA", "custom_schema");

        try
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost;Database=test;Username=u;Password=p")
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Model.GetDefaultSchema().Should().Be("custom_schema");
        }
        finally
        {
            Environment.SetEnvironmentVariable("DATABASE_SCHEMA", oldSchema);
        }
    }
}
