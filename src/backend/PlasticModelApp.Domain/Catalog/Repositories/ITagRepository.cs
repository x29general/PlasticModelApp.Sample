using PlasticModelApp.Domain.Catalog.Entities;
using PlasticModelApp.Domain.Catalog.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.Repositories;

/// <summary>
/// Repository Interface for Tag Entity.
/// Defines the contract for Tag repository operations.
/// </summary>
public interface ITagRepository
{
    /// <summary>
    /// Checks if a Tag with the specified ID exists.
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>True if the Tag exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(TagId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a Tag by its ID.
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>The Tag entity if found; otherwise, null.</returns>
    Task<Tag?> GetByIdAsync(TagId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new Tag to the repository.
    /// </summary>
    /// <param name="tag">Tag entity to add</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(Tag tag, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing Tag in the repository.
    /// </summary>
    /// <param name="tag">Tag entity to update</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Tag tag, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a Tag as deleted in the repository.
    /// </summary>
    /// <param name="tag">Tag entity to mark as deleted</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task MarkAsDeletedAsync(Tag tag, CancellationToken cancellationToken = default);
}
