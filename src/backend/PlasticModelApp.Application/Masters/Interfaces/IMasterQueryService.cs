using PlasticModelApp.Application.Masters.Queries;

namespace PlasticModelApp.Application.Masters.Interfaces;

/// <summary>
/// Interface for querying master data.
/// </summary>
public interface IMasterQueryService
{
    /// <summary>
    /// Gets all master data.
    /// </summary>
    Task<GetMastersResult> GetAllAsync(CancellationToken cancellationToken = default);
}
