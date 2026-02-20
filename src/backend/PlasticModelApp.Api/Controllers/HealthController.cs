using Microsoft.AspNetCore.Mvc;
using PlasticModelApp.Api.Contracts.Responses;
using PlasticModelApp.Infrastructure.Data;

namespace PlasticModelApp.Api.Controllers;

[ApiController]
[Route("")]
public sealed class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public HealthController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("healthz")]
    public ActionResult<object> GetHealth()
    {
        return Ok(new { Status = "ok" });
    }

    [HttpGet("health/db")]
    public async Task<ActionResult<DatabaseHealthResponse>> GetDatabaseHealth(CancellationToken cancellationToken)
    {
        try
        {
            var isConnected = await _dbContext.Database.CanConnectAsync(cancellationToken);
            if (isConnected)
            {
                return Ok(new DatabaseHealthResponse { status = "ok" });
            }

            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new DatabaseHealthResponse { status = "error", message = "database is not reachable" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new DatabaseHealthResponse { status = "error", message = ex.Message });
        }
    }
}
