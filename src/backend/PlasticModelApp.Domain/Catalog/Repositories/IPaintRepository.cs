using PlasticModelApp.Domain.Catalog.Entities;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.Repositories;

/// <summary>
/// Repository Interface for Paint Entity.
/// Defines the contract for Paint repository operations.
/// </summary>
public interface IPaintRepository
{
    /// <summary>
    /// Checks if a Paint with the specified ID exists.
    /// </summary>
    /// <param name="id">Paint ID</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>True if the Paint exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(PaintId id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a Paint by its ID.
    /// </summary>
    /// <param name="id">Paint ID</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>The Paint entity if found; otherwise, null.</returns>
    Task<Paint?> GetByIdAsync(PaintId id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new Paint to the repository.
    /// </summary>
    /// <param name="paint">Paint entity to add</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task AddAsync(Paint paint, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing Paint in the repository.
    /// </summary>
    /// <param name="paint">Paint entity to update</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task UpdateAsync(Paint paint, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a Paint as deleted in the repository.
    /// </summary>
    /// <param name="paint">Paint entity to mark as deleted</param>
    /// <param name="cancellationToken" >Cancellation Token</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task MarkAsDeletedAsync(Paint paint, CancellationToken cancellationToken);
}

