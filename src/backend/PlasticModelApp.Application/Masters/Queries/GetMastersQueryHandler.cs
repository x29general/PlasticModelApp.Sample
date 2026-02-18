using MediatR;
using PlasticModelApp.Application.Masters.Interfaces;
using Microsoft.Extensions.Logging;

namespace PlasticModelApp.Application.Masters.Queries;

/// <summary>
/// Handles the GetMastersQuery to retrieve master data.
/// This handler uses the IMasterQueryService to fetch all master data and logs the process.
/// </summary>
public class GetMastersQueryHandler : IRequestHandler<GetMastersQuery, GetMastersResult>
{
    /// <summary>
    /// Service for querying master data. This service provides methods to retrieve various types of master data, such as brands, paint types, glosses, tags, and tag categories.
    /// </summary>
    private readonly IMasterQueryService _queryService;

    /// <summary>
    /// Logger for logging the process of retrieving master data. 
    /// This logger is used to log information about the start and completion of the data retrieval, as well as any errors that may occur during the process.
    /// </summary>
    private readonly ILogger<GetMastersQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the GetMastersQueryHandler class with the specified IMasterQueryService and ILogger.
    /// </summary>
    /// <param name="queryService">The service used to query master data.</param>
    /// <param name="logger">The logger used to log the process of retrieving master data.</param>
    public GetMastersQueryHandler(
        IMasterQueryService queryService,
        ILogger<GetMastersQueryHandler> logger)
    {
        _queryService = queryService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetMastersQuery request.
    /// This method retrieves all master data using the IMasterQueryService and logs the process.
    /// If the retrieval is successful, it logs the counts of each type of master data retrieved and returns the result. 
    /// If an error occurs, it logs the error message and rethrows the exception.
    /// </summary>
    /// <param name="request">The GetMastersQuery request containing any necessary parameters for retrieving master data.</param>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// <returns>Returns a GetMastersResult containing the retrieved master data.</returns>
    public async Task<GetMastersResult> Handle(
        GetMastersQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("マスターデータの一括取得開始");

        try
        {
            var mappedResult = await _queryService.GetAllAsync(cancellationToken);

            _logger.LogInformation("マスターデータの一括取得完了: brands={BrandCount}, types={TypeCount}, glosses={GlossCount}, tags={TagCount}, tagCategories={TagCategoryCount}", 
                mappedResult.Brands?.Count ?? 0,
                mappedResult.PaintTypes?.Count ?? 0,
                mappedResult.Glosses?.Count ?? 0,
                mappedResult.Tags?.Count ?? 0,
                mappedResult.TagCategories?.Count ?? 0);

            return mappedResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "マスターデータ取得失敗: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
