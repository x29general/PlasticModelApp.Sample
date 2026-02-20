using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Application.Tags.Interfaces;
using PlasticModelApp.Application.Tags.Queries;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Infrastructure.Data;

namespace PlasticModelApp.Infrastructure.Queries;

/// <summary>
/// Query service for tags.
/// </summary>
public sealed class TagQueryService : ITagQueryService
{
    private readonly ApplicationDbContext _db;

    public TagQueryService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<GetTagByIdResult?> GetByIdAsync(TagId id, CancellationToken cancellationToken = default)
    {
        var tagId = id.Value;

        return await _db.Tags
            .AsNoTracking()
            .Where(t => t.Id == tagId)
            .Select(t => new GetTagByIdResult
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Hex = t.Hex,
                Effect = t.Effect,
                CategoryId = t.TagCategoryId,
                CreatedAt = t.CreatedAt.UtcDateTime,
                UpdatedAt = t.UpdatedAt.UtcDateTime,
                IsDeleted = t.IsDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<GetTagByIdResult>> GetByCategoryAsync(string? category, CancellationToken cancellationToken = default)
    {
        var query = _db.Tags
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(t => t.TagCategory != null && t.TagCategory.Name == category);
        }

        return await query
            .OrderBy(t => t.Name)
            .Select(t => new GetTagByIdResult
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Hex = t.Hex,
                Effect = t.Effect,
                CategoryId = t.TagCategoryId,
                CreatedAt = t.CreatedAt.UtcDateTime,
                UpdatedAt = t.UpdatedAt.UtcDateTime,
                IsDeleted = t.IsDeleted
            })
            .ToListAsync(cancellationToken);
    }
}
