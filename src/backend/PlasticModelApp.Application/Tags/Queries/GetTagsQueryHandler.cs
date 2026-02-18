using MediatR;
using PlasticModelApp.Application.Tags.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PlasticModelApp.Application.Tags.Queries;

/// <summary>
/// Handler for processing `GetTagsQuery`. This handler interacts with the `ITagQueryService` to retrieve a list of tags based on the specified category filter.
/// </summary>
public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, GetTagsResult>
{
    /// <summary>
    /// Service for querying tag information. This service provides methods to retrieve tags by their ID or by their category.
    /// </summary>
    private readonly ITagQueryService _queryService;

    /// <summary>
    /// Logger for logging the process of retrieving tags.
    /// This logger can be used to log information about the start and completion of the tag retrieval process, as well as any errors that may occur during the operation.
    /// </summary>
    private readonly ILogger<GetTagsQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the `GetTagsQueryHandler` class with the specified `ITagQueryService`.
    /// </summary>
    /// <param name="queryService">The service used to query tag information.</param>
    public GetTagsQueryHandler(ITagQueryService queryService, ILogger<GetTagsQueryHandler> logger)
    {
        _queryService = queryService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the `GetTagsQuery` by retrieving a list of tags that match the specified category filter using the `ITagQueryService`.
    /// The result includes a list of tag information and the total count of tags that match the criteria.
    /// </summary>
    /// <param name="request">The query request containing the optional category filter.</param>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// returns>Returns a `GetTagsResult` containing a list of tags and the total count of matching tags.</returns>
    public async Task<GetTagsResult> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("タグの取得開始: category={Category}", request.Category ?? "null");

        try
        {
            var tags = await _queryService.GetByCategoryAsync(request.Category, cancellationToken);

            _logger.LogInformation("タグの取得完了: category={Category}, count={Count}", request.Category ?? "null", tags.Count);
            return new GetTagsResult
            {
                Items = tags,
                Total = tags.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "タグの取得中にエラーが発生: category={Category}", request.Category ?? "null");
            throw;
        }
    }
}
