using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Application.Tags.Queries;

namespace PlasticModelApp.Application.Tags.Interfaces;

/// <summary>
/// Interface for querying tag information. This service provides methods to retrieve tags by their ID or by their category.
/// </summary>
public interface ITagQueryService
{
    /// <summary>
    /// Gets a tag by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the tag.</param>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// <returns>Returns the tag information if found; otherwise, returns null.</returns>
    Task<GetTagByIdResult?> GetByIdAsync(TagId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of tags that belong to a specific category. If the category is null, it returns all tags.
    /// </summary>
    /// <param name="category">The category to filter tags by. If null, all tags are returned.</param>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// <returns>Returns a list of tags that match the specified category.</returns>
    Task<List<GetTagByIdResult>> GetByCategoryAsync(string? category, CancellationToken cancellationToken = default);
}
