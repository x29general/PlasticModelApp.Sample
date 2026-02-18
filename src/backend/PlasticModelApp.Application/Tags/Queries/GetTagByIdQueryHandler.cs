using MediatR;
using PlasticModelApp.Application.Tags.Interfaces;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Application.Shared.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PlasticModelApp.Application.Tags.Queries;

/// <summary>
/// Query handler for `GetTagByIdQuery`. This handler processes the query to retrieve a tag by its unique identifier. 
/// It uses the `ITagQueryService` to fetch the tag information and handles cases where the tag is not found or is marked as deleted.
/// </summary>
public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, GetTagByIdResult>
{
    /// <summary>
    /// Service for querying tag information. This service provides methods to retrieve tags by their ID or by their category.
    /// The `GetTagByIdQueryHandler` relies on this service to fetch the tag information based on the provided ID in the query.
    /// </summary>
    private readonly ITagQueryService _queryService;

    /// <summary>
    /// Logger for logging the process of retrieving a tag by its ID. 
    /// This logger can be used to log information about the start and completion of the retrieval process, as well as any errors that may occur during the operation.
    /// </summary>
    private readonly ILogger<GetTagByIdQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the `GetTagByIdQueryHandler` class with the specified `ITagQueryService` and `ILogger`.
    /// The constructor takes in the query service and logger as dependencies, which are used to handle the retrieval of tag information and to log the process respectively.
    /// </summary>
    /// <param name="queryService">The service used to query tag information.</param>
    /// <param name="logger">The logger used to log the retrieval process.</param>
    public GetTagByIdQueryHandler(ITagQueryService queryService, ILogger<GetTagByIdQueryHandler> logger)
    {
        _queryService = queryService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the `GetTagByIdQuery` by retrieving the tag information using the `ITagQueryService`.
    /// If the tag is not found or is marked as deleted, it throws a `NotFoundException` with a specific error code and message. Otherwise, it returns the tag information as a `GetTagByIdResult`.
    /// </summary>
    /// <param name="request">The query request containing the ID of the tag to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// <returns>Returns the tag information if found and not deleted; otherwise, throws a `NotFoundException`.</returns>
    /// <exception cref="NotFoundException">Thrown when the tag is not found or is marked as deleted.</exception>
    public async Task<GetTagByIdResult> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("タグの取得開始: id={Id}", request.Id);

        try
        {
            var result = await _queryService.GetByIdAsync(new TagId(request.Id), cancellationToken);
            if (result == null || result.IsDeleted)
            {
                _logger.LogWarning("タグが見つかりません: id={Id}", request.Id);
                throw new NotFoundException("E-404-001", "見つかりません:要求されたリソースが存在しません。", new object[] { request.Id }, DateTime.UtcNow);
            }

            _logger.LogInformation("タグの取得完了: id={Id}", request.Id);
            return result;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "タグの取得中にエラーが発生: id={Id}", request.Id);
            throw;
        }
    }
}
