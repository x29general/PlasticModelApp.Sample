using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Application.Masters.Interfaces;
using PlasticModelApp.Application.Masters.Queries;
using PlasticModelApp.Application.Tags.Queries;
using PlasticModelApp.Infrastructure.Data;

namespace PlasticModelApp.Infrastructure.Queries;

/// <summary>
/// Implementation of IMasterQueryService that retrieves master data from the database.
/// </summary>
public sealed class MasterQueryService : IMasterQueryService
{
    /// <summary>
    /// The database context used to access the master data.
    /// </summary>
    private readonly ApplicationDbContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="MasterQueryService"/> class.
    /// </summary>
    /// <param name="db">The database context to be used by the service.</param>
    public MasterQueryService(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<GetMastersResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _db.Brands
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .Select(b => new GetBrandResult
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            })
            .ToListAsync(cancellationToken);

        var paintTypes = await _db.PaintTypes
            .AsNoTracking()
            .OrderBy(pt => pt.Name)
            .Select(pt => new GetPaintTypeResult
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description
            })
            .ToListAsync(cancellationToken);

        var glosses = await _db.Glosses
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .Select(g => new GetGlossResult
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description
            })
            .ToListAsync(cancellationToken);

        var tagCategories = await _db.TagCategories
            .AsNoTracking()
            .OrderBy(tc => tc.Name)
            .Select(tc => new GetTagCategoryResult
            {
                Id = tc.Id,
                Name = tc.Name,
                Description = tc.Description
            })
            .ToListAsync(cancellationToken);

        var tags = await _db.Tags
            .AsNoTracking()
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

        return new GetMastersResult
        {
            Brands = brands,
            Glosses = glosses,
            PaintTypes = paintTypes,
            Tags = tags,
            TagCategories = tagCategories
        };
    }
}
