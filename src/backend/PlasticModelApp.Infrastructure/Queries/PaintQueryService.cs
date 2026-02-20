using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Application.Tags.Queries;
using PlasticModelApp.Domain.Catalog.Services;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using PlasticModelApp.Infrastructure.Data;

namespace PlasticModelApp.Infrastructure.Queries;

/// <summary>
/// Query service for paints.
/// </summary>
public sealed class PaintQueryService : IPaintQueryService
{
    /// <summary>
    /// The database context used to access the paint data.
    /// </summary>
    private readonly ApplicationDbContext _db;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaintQueryService"/> class.
    /// </summary>
    /// <param name="db">The database context to be used by the service.</param>
    public PaintQueryService(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<GetPaintByIdResult?> GetByIdAsync(PaintId id, CancellationToken cancellationToken = default)
    {
        var paintId = id.Value;

        var paint = await _db.Paints
            .AsNoTracking()
            .Where(p => p.Id == paintId)
            .Select(p => new PaintDetailsProjection
            {
                Id = p.Id,
                Name = p.Name,
                ModelNumber = p.ModelNumber,
                BrandId = p.BrandId,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty,
                PaintTypeId = p.PaintTypeId,
                PaintTypeName = p.PaintType != null ? p.PaintType.Name : string.Empty,
                GlossId = p.GlossId,
                GlossName = p.Gloss != null ? p.Gloss.Name : string.Empty,
                Price = p.Price,
                Description = p.Description,
                Hex = p.Hex,
                RgbR = p.RgbR,
                RgbG = p.RgbG,
                RgbB = p.RgbB,
                HslH = p.HslH,
                HslS = p.HslS,
                HslL = p.HslL,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsDeleted = p.IsDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (paint is null)
        {
            return null;
        }

        var tagMap = await BuildTagMapAsync([paint.Id], cancellationToken);

        return ToPaintDetailsDto(
            paint,
            tagMap.TryGetValue(paint.Id, out var tags) ? tags : []);
    }

    /// <inheritdoc/>
    public async Task<SearchPaintsResult> SearchAsync(SearchPaintsQuery searchPaintsQuery, CancellationToken cancellationToken = default)
    {
        var c = searchPaintsQuery.Criteria;
        c.Normalize();

        var query = _db.Paints
            .AsNoTracking()
            .AsQueryable();

        if (c.BrandIds.Count > 0)
        {
            var brandIds = c.BrandIds.Distinct().ToList();
            query = query.Where(p => brandIds.Contains(p.BrandId));
        }

        if (c.PaintTypeIds.Count > 0)
        {
            var paintTypeIds = c.PaintTypeIds.Distinct().ToList();
            query = query.Where(p => paintTypeIds.Contains(p.PaintTypeId));
        }

        if (c.GlossIds.Count > 0)
        {
            var glossIds = c.GlossIds.Distinct().ToList();
            query = query.Where(p => glossIds.Contains(p.GlossId));
        }

        if (c.TagIds.Count > 0)
        {
            var selectedTagIds = c.TagIds.Distinct().ToList();

            var tagCategoryPairs = await _db.Tags
                .AsNoTracking()
                .Where(t => selectedTagIds.Contains(t.Id))
                .Select(t => new { t.TagCategoryId, t.Id })
                .ToListAsync(cancellationToken);

            var groups = tagCategoryPairs
                .GroupBy(x => x.TagCategoryId)
                .Select(g => g.Select(x => x.Id).ToList())
                .ToList();

            foreach (var group in groups)
            {
                var groupIds = group;
                query = query.Where(p => _db.PaintTags.Any(pt => pt.PaintId == p.Id && groupIds.Contains(pt.TagId)));
            }
        }

        IQueryable<string> orderedIdsQuery;
        if (c.Sort is SearchPaintsSortOption.ModelNumberAsc or SearchPaintsSortOption.ModelNumberDesc)
        {
            var joined =
                from p in query
                join b in _db.Brands.AsNoTracking() on p.BrandId equals b.Id into bj
                from b in bj.DefaultIfEmpty()
                select new
                {
                    p.Id,
                    BrandName = b != null ? b.Name : string.Empty,
                    p.ModelNumberPrefix,
                    p.ModelNumberNumber
                };

            orderedIdsQuery = c.Sort == SearchPaintsSortOption.ModelNumberAsc
                ? joined
                    .OrderBy(x => x.BrandName)
                    .ThenBy(x => x.ModelNumberPrefix)
                    .ThenBy(x => x.ModelNumberNumber)
                    .ThenBy(x => x.Id)
                    .Select(x => x.Id)
                : joined
                    .OrderByDescending(x => x.BrandName)
                    .ThenByDescending(x => x.ModelNumberPrefix)
                    .ThenByDescending(x => x.ModelNumberNumber)
                    .ThenBy(x => x.Id)
                    .Select(x => x.Id);
        }
        else
        {
            orderedIdsQuery = c.Sort switch
            {
                SearchPaintsSortOption.NameAsc => query.OrderBy(p => p.Name).ThenBy(p => p.Id).Select(p => p.Id),
                SearchPaintsSortOption.NameDesc => query.OrderByDescending(p => p.Name).ThenBy(p => p.Id).Select(p => p.Id),
                _ => query.OrderBy(p => p.Name).ThenBy(p => p.Id).Select(p => p.Id)
            };
        }

        var total = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var skip = (c.Page - 1) * c.PageSize;

        var orderedIds = await orderedIdsQuery
            .Skip(skip)
            .Take(c.PageSize)
            .ToListAsync(cancellationToken);

        if (orderedIds.Count == 0)
        {
            return new SearchPaintsResult
            {
                Items = [],
                Total = total
            };
        }

        var itemMap = await _db.Paints
            .AsNoTracking()
            .Where(p => orderedIds.Contains(p.Id))
            .Select(p => new SearchPaintsResultItem
            {
                Id = p.Id,
                Name = p.Name,
                ModelNumber = p.ModelNumber,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty,
                HexColor = p.Hex
            })
            .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

        var items = orderedIds
            .Select(id => itemMap.TryGetValue(id, out var dto) ? dto : null)
            .Where(x => x is not null)
            .Select(x => x!)
            .ToList();

        return new SearchPaintsResult
        {
            Items = items,
            Total = total
        };
    }

    /// <inheritdoc/>
    public async Task<SimilarPaintsResult> SearchSimilarAsync(SearchSimilarPaintsQuery query, CancellationToken cancellationToken = default)
    {
        query.Normalize();
        var target = query.TargetColor;
        const int MaxResultWindow = 100;

        var paints = await _db.Paints
            .AsNoTracking()
            .Select(p => new SimilarPaintProjection
            {
                Id = p.Id,
                Name = p.Name,
                ModelNumber = p.ModelNumber,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty,
                Hex = p.Hex,
                RgbR = p.RgbR,
                RgbG = p.RgbG,
                RgbB = p.RgbB
            })
            .ToListAsync(cancellationToken);

        if (paints.Count == 0)
        {
            return new SimilarPaintsResult
            {
                Items = [],
                Total = 0,
            };
        }

        var scored = paints
            .Select(p => new SimilarSearchPaintResultItem
            {
                Id = p.Id,
                Name = p.Name,
                ModelNumber = p.ModelNumber,
                BrandName = p.BrandName,
                HexColor = p.Hex,
                Similarity = ColorDifferenceCalculator.CalculateCiede2000(
                    p.RgbR,
                    p.RgbG,
                    p.RgbB,
                    target.R,
                    target.G,
                    target.B)
            })
            .Where(sp => sp.Similarity <= query.Threshold)
            .OrderBy(sp => sp.Similarity)
            .ThenBy(sp => sp.Name);

        var limited = scored.Take(MaxResultWindow).ToList();
        var skip = (query.Page - 1) * query.PageSize;
        var paged = limited
            .Skip(skip)
            .Take(query.PageSize)
            .ToList();

        return new SimilarPaintsResult
        {
            Items = paged,
            Total = limited.Count
        };
    }

    public async Task<IReadOnlyList<GetPaintByIdResult>> ListByIdsAsync(
        IReadOnlyCollection<PaintId> ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        if (ids.Count == 0)
        {
            return Array.Empty<GetPaintByIdResult>();
        }

        var orderedDistinctIds = ids
            .Select(x => x.Value)
            .Distinct()
            .ToList();

        var paints = await _db.Paints
            .AsNoTracking()
            .Where(p => orderedDistinctIds.Contains(p.Id))
            .Select(p => new PaintDetailsProjection
            {
                Id = p.Id,
                Name = p.Name,
                ModelNumber = p.ModelNumber,
                BrandId = p.BrandId,
                BrandName = p.Brand != null ? p.Brand.Name : string.Empty,
                PaintTypeId = p.PaintTypeId,
                PaintTypeName = p.PaintType != null ? p.PaintType.Name : string.Empty,
                GlossId = p.GlossId,
                GlossName = p.Gloss != null ? p.Gloss.Name : string.Empty,
                Price = p.Price,
                Description = p.Description,
                Hex = p.Hex,
                RgbR = p.RgbR,
                RgbG = p.RgbG,
                RgbB = p.RgbB,
                HslH = p.HslH,
                HslS = p.HslS,
                HslL = p.HslL,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsDeleted = p.IsDeleted
            })
            .ToListAsync(cancellationToken);

        if (paints.Count == 0)
        {
            return Array.Empty<GetPaintByIdResult>();
        }

        var tagMap = await BuildTagMapAsync(orderedDistinctIds, cancellationToken).ConfigureAwait(false);
        var mapped = paints.ToDictionary(
            x => x.Id,
            x => ToPaintDetailsDto(x, tagMap.TryGetValue(x.Id, out var tags) ? tags : []));

        return orderedDistinctIds
            .Select(id => mapped.TryGetValue(id, out var dto) ? dto : null)
            .Where(x => x is not null)
            .Select(x => x!)
            .ToList();
    }

    /// <summary>
    /// Builds a map from paint ID to its associated tags.
    /// </summary>
    /// <param name="paintIds">The collection of paint IDs for which to build the tag map.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary mapping paint IDs to their associated tags.</returns>
    private async Task<Dictionary<string, List<GetTagByIdResult>>> BuildTagMapAsync(
        IReadOnlyCollection<string> paintIds,
        CancellationToken cancellationToken)
    {
        if (paintIds.Count == 0)
        {
            return new Dictionary<string, List<GetTagByIdResult>>();
        }

        var rows = await (
            from pt in _db.PaintTags.AsNoTracking()
            join t in _db.Tags.AsNoTracking() on pt.TagId equals t.Id
            where paintIds.Contains(pt.PaintId)
            orderby t.Name
            select new
            {
                pt.PaintId,
                t.Id,
                t.Name,
                t.Description,
                t.Hex,
                t.Effect,
                t.TagCategoryId,
                t.CreatedAt,
                t.UpdatedAt,
                t.IsDeleted
            })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(x => x.PaintId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(t => new GetTagByIdResult
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
                }).ToList());
    }

    /// <summary>
    /// Converts a <see cref="PaintDetailsProjection"/> and its associated tags to a <see cref="GetPaintByIdResult"/> DTO.
    /// </summary>
    /// <param name="source">The source <see cref="PaintDetailsProjection"/> containing the paint details.</param>
    /// <param name="tags">The list of associated tags for the paint.</param>
    /// <returns>A <see cref="GetPaintByIdResult"/> DTO representing the paint details and its associated tags.</returns>
    private static GetPaintByIdResult ToPaintDetailsDto(
        PaintDetailsProjection source,
        List<GetTagByIdResult> tags)
    {
        return new GetPaintByIdResult
        {
            Id = source.Id,
            Name = source.Name,
            ModelNumber = source.ModelNumber,
            BrandId = source.BrandId,
            BrandName = source.BrandName,
            PaintTypeId = source.PaintTypeId,
            PaintTypeName = source.PaintTypeName,
            GlossId = source.GlossId,
            GlossName = source.GlossName,
            Price = source.Price,
            Description = source.Description,
            Hex = source.Hex,
            Rgb_R = source.RgbR,
            Rgb_G = source.RgbG,
            Rgb_B = source.RgbB,
            Hsl_H = source.HslH,
            Hsl_S = source.HslS,
            Hsl_L = source.HslL,
            ImageUrl = source.ImageUrl,
            Tags = tags,
            CreatedAt = source.CreatedAt.UtcDateTime,
            UpdatedAt = source.UpdatedAt.UtcDateTime,
            IsDeleted = source.IsDeleted,
        };
    }

    // The following are private projection classes used for intermediate query results.
    private sealed class PaintDetailsProjection
    {
        public string Id { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string ModelNumber { get; init; } = string.Empty;

        public string BrandId { get; init; } = string.Empty;

        public string BrandName { get; init; } = string.Empty;

        public string PaintTypeId { get; init; } = string.Empty;

        public string PaintTypeName { get; init; } = string.Empty;

        public string GlossId { get; init; } = string.Empty;

        public string GlossName { get; init; } = string.Empty;

        public decimal Price { get; init; }

        public string? Description { get; init; }

        public string Hex { get; init; } = string.Empty;

        public int RgbR { get; init; }

        public int RgbG { get; init; }

        public int RgbB { get; init; }

        public float HslH { get; init; }

        public float HslS { get; init; }

        public float HslL { get; init; }

        public string? ImageUrl { get; init; }

        public DateTimeOffset CreatedAt { get; init; }

        public DateTimeOffset UpdatedAt { get; init; }

        public bool IsDeleted { get; init; }
    }

    private sealed class SimilarPaintProjection
    {
        public string Id { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string ModelNumber { get; init; } = string.Empty;

        public string BrandName { get; init; } = string.Empty;

        public string Hex { get; init; } = string.Empty;

        public int RgbR { get; init; }

        public int RgbG { get; init; }

        public int RgbB { get; init; }
    }
}
