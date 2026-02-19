using MediatR;
using Microsoft.Extensions.Logging;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Shared.Exceptions;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Query handler for searching similar paints based on a given paint and threshold. 
/// It handles the logic of invoking the paint query service and logging the process, 
/// while also managing exceptions that may occur during the search operation.
/// </summary>
public sealed class SearchSimilarPaintsQueryHandler : IRequestHandler<SearchSimilarPaintsQuery, SimilarPaintsResult>
{
    /// <summary>
    /// The paint query service used to perform the search for similar paints.
    /// </summary>
    private readonly IPaintQueryService _queryService;

    /// <summary>
    /// Logger for logging the search process and any errors that may occur during the similar paint search operation.
    /// </summary>
    private readonly ILogger<SearchSimilarPaintsQueryHandler> _logger;

    /// <summary>
    /// Constructor for the SearchSimilarPaintsQueryHandler, which initializes the paint query service and logger.
    /// </summary>
    /// <param name="queryService">The paint query service to be used for searching similar paints.</param>
    /// <param name="logger">The logger for logging the search process and any errors that may occur during the similar paint search operation.</param>
    public SearchSimilarPaintsQueryHandler(IPaintQueryService queryService, ILogger<SearchSimilarPaintsQueryHandler> logger)
    {
        _queryService = queryService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the SearchSimilarPaintsQuery by invoking the paint query service to search for similar paints based on the provided threshold, page, and page size.
    /// It also logs the start and completion of the search process, as well as any errors that may occur during the operation.
    /// </summary>
    /// <param name="request">The SearchSimilarPaintsQuery containing the parameters for the search operation, such as threshold, page, and page size.</param>
    /// <param name="cancellationToken">The cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A SimilarPaintsResult containing the results of the similar paint search operation.</returns>
    /// <exception cref="ValidationException">Thrown when the input parameters for the search operation are invalid.</exception>
    /// <exception cref="AppSystemException">Thrown when a generic error occurs on the server during the similar paint search operation.</exception>
    public async Task<SimilarPaintsResult> Handle(SearchSimilarPaintsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            request.Normalize();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "不正なリクエスト: {Message}", ex.Message);
            throw new ValidationException("E-400-001", "不正なリクエスト:リクエストに無効な値が含まれるか、必須パラメータが欠落しています。", Array.Empty<object>(), DateTime.UtcNow);
        }

        _logger.LogInformation("類似色検索開始: threshold={Threshold}, page={Page}, size={Size}", request.Threshold, request.Page, request.PageSize);

        try
        {
            var result = await _queryService.SearchSimilarAsync(request, cancellationToken);
            _logger.LogInformation("類似色検索完了: total={Total}", result.Total);
            return result;
        }
        catch (ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "類似色検索中にエラー");
            throw new AppSystemException("E-500-01", "Internal Server Error: A generic error occurred on the server.", Array.Empty<object>(), DateTime.UtcNow);
        }
    }
}

