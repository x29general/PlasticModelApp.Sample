using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Infrastructure.Data.Entities;

namespace PlasticModelApp.Infrastructure.Data;

/// <summary>
/// Application database context.
/// </summary>
public class ApplicationDbContext : DbContext
{
    private const string PostgreSqlProviderName = "Npgsql.EntityFrameworkCore.PostgreSQL";

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a DbContext.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PaintEntity> Paints => Set<PaintEntity>();

    public DbSet<BrandEntity> Brands => Set<BrandEntity>();

    public DbSet<PaintTypeEntity> PaintTypes => Set<PaintTypeEntity>();

    public DbSet<GlossEntity> Glosses => Set<GlossEntity>();

    public DbSet<TagEntity> Tags => Set<TagEntity>();

    public DbSet<TagCategoryEntity> TagCategories => Set<TagCategoryEntity>();

    public DbSet<PaintTagEntity> PaintTags => Set<PaintTagEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var dbSchema = Environment.GetEnvironmentVariable("DATABASE_SCHEMA") ?? "public";
        var providerName = Database.ProviderName ?? string.Empty;

        if (string.Equals(providerName, PostgreSqlProviderName, StringComparison.OrdinalIgnoreCase))
        {
            modelBuilder.HasDefaultSchema(dbSchema);
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
