using MediatR;
using Microsoft.Extensions.Logging;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Shared.Exceptions;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Handler for `SearchPaintsQuery`, which processes search requests for paints based on specified criteria and returns paginated results. 
/// It validates the search criteria, logs the search process, and handles any exceptions that may occur during the search operation.
/// </summary>
public class SearchPaintsQueryHandler : IRequestHandler<SearchPaintsQuery, SearchPaintsResult>
{
    /// <summary>
    /// Paint query service used to perform the search operation based on the provided criteria.
    /// </summary>
    private readonly IPaintQueryService _queryService;

    /// <summary>
    /// Logger for `SearchPaintsQueryHandler`, which is used to log information about the search process, including the search criteria and any errors that occur during the search.
    /// </summary>
    private readonly ILogger<SearchPaintsQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of `SearchPaintsQueryHandler` with the specified paint query service and logger.
    /// </summary>
    /// <param name="queryService">The paint query service used to perform search operations.</param>
    /// <param name="logger">The logger used to log information about the search process.</param>
    public SearchPaintsQueryHandler(
        IPaintQueryService queryService,
        ILogger<SearchPaintsQueryHandler> logger)
    {
        _queryService = queryService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the `SearchPaintsQuery` by validating the search criteria, logging the search process, and invoking the paint query service to perform the search based on the specified criteria.
    /// It also handles any exceptions that may occur during the search operation and logs appropriate messages for invalid criteria or search errors.
    /// </summary>
    /// <param name="request">The search query containing the criteria for searching paints.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>A `SearchPaintsResult` containing the list of paints that match the search criteria and the total count of matching paints.</returns>
    /// <exception cref="ValidationException">Thrown when the search criteria are invalid.</exception>
    /// <exception cref="AppSystemException">Thrown when an error occurs during the search operation.</exception>
    public async Task<SearchPaintsResult> Handle(SearchPaintsQuery request, CancellationToken cancellationToken)
    {
        var c = request.Criteria;

        try
        {
            c.Normalize();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "不正なリクエスト: {Message}", ex.Message);
            throw new ValidationException("E-400-001", "不正なリクエスト:リクエストに無効な値が含まれるか、必須パラメータが欠落しています。", Array.Empty<object>(), DateTime.UtcNow);
        }

        _logger.LogInformation(
            "塗料検索開始: brands={BrandCount}, types={TypeCount}, glosses={GlossCount}, tags={TagCount}, sort={Sort}, page={Page}, size={Size}",
            c.BrandIds.Count,
            c.PaintTypeIds.Count,
            c.GlossIds.Count,
            c.TagIds.Count,
            c.Sort,
            c.Page,
            c.PageSize);

        try
        {
            // Execute the search
            var result = await _queryService.SearchAsync(request, cancellationToken);
            _logger.LogInformation("塗料検索完了: total={Total}", result.Total);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "塗料検索中にエラー: {Message}", ex.Message);
            throw new AppSystemException("E-500-01", "Internal Server Error: A generic error occurred on the server.", Array.Empty<object>(), DateTime.UtcNow);
        }
    }
}
