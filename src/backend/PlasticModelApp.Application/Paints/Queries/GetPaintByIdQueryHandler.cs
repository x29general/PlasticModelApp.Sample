using MediatR;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Application.Shared.Exceptions;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Logging;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Handler for `GetPaintByIdQuery`.
/// </summary>
public class GetPaintByIdQueryHandler : IRequestHandler<GetPaintByIdQuery, GetPaintByIdResult>
{
    /// <summary>
    /// Paint query service.
    /// </summary>
    private readonly IPaintQueryService _queryService;

    /// <summary>
    /// Logger for `GetPaintByIdQueryHandler`.
    /// </summary>
    private readonly ILogger<GetPaintByIdQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of `GetPaintByIdQueryHandler`.
    /// </summary>
    /// <param name="queryService">Paint query service.</param>
    public GetPaintByIdQueryHandler(
        IPaintQueryService queryService,
        ILogger<GetPaintByIdQueryHandler> logger)
    {
        _queryService = queryService;
        _logger = logger;
    }

    /// <summary>
    /// Handles a query that gets a paint by ID.
    /// </summary>
    /// <param name="query">Query with the paint ID to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Result containing the paint details.</returns>
    public async Task<GetPaintByIdResult> Handle(
        GetPaintByIdQuery query, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("塗料の取得開始: id={Id}", query.Id);

        try
        {
            var result = await _queryService.GetByIdAsync(new PaintId(query.Id), cancellationToken);
            if (result == null || result.IsDeleted)
            {
                _logger.LogWarning("塗料が見つかりません: id={Id}", query.Id);
                throw new NotFoundException("E-404-001", "見つかりません:要求されたリソースが存在しません。", new object[] { query.Id }, DateTime.UtcNow);
            }
            return result;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "塗料の取得中にエラーが発生: id={Id}", query.Id);
            throw;
        }
    }
}

