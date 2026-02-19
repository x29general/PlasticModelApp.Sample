using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Application.Paints.Interfaces;

/// <summary>
/// Query interface for paint data.
/// </summary>
public interface IPaintQueryService
{
    /// <summary>
    /// Gets a paint by ID.
    /// </summary>
    /// <param name="id">Paint ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paint details if found; otherwise, null.</returns>
    Task<GetPaintByIdResult?> GetByIdAsync(PaintId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paged paints by search conditions.
    /// </summary>
    /// <param name="searchPaintsQuery">Query with search criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paged list of paints matching the search criteria.</returns>
    Task<SearchPaintsResult> SearchAsync(SearchPaintsQuery searchPaintsQuery, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches paints by color similarity.
    /// </summary>
    /// <param name="query">Query with color information for similarity search.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of paints similar in color to the specified criteria.</returns>
    Task<SimilarPaintsResult> SearchSimilarAsync(SearchSimilarPaintsQuery query, CancellationToken cancellationToken = default);

    // /// <summary>
    // /// Gets a list of paints by their IDs.
    // /// </summary>
    // /// <param name="ids">Collection of paint IDs.</param>
    // /// <param name="cancellationToken">Cancellation token.</param>
    // /// <returns>List of paints corresponding to the provided IDs.</returns>
    // Task<IReadOnlyList<GetPaintByIdResult>> ListByIdsAsync(IReadOnlyCollection<PaintId> ids, CancellationToken cancellationToken = default);
}


