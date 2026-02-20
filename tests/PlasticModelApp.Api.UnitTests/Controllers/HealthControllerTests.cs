using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlasticModelApp.Api.Controllers;
using PlasticModelApp.Infrastructure.Data;

namespace PlasticModelApp.Api.UnitTests.Controllers;

public class HealthControllerTests
{
    [Fact]
    public void GetHealth_ShouldReturnOkStatus()
    {
        using var db = CreateDbContext();
        var controller = new HealthController(db);

        var result = controller.GetHealth();

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().NotBeNull();
        ok.Value!.GetType().GetProperty("Status")!.GetValue(ok.Value)!.Should().Be("ok");
    }

    [Fact]
    public async Task GetDatabaseHealth_WhenConnected_ShouldReturnOk()
    {
        await using var db = CreateDbContext();
        var controller = new HealthController(db);

        var result = await controller.GetDatabaseHealth(CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value.Should().BeAssignableTo<PlasticModelApp.Api.Contracts.Responses.DatabaseHealthResponse>().Subject;
        response.status.Should().Be("ok");
    }

    [Fact]
    public async Task GetDatabaseHealth_WhenException_ShouldReturn503()
    {
        var db = CreateDbContext();
        db.Dispose();
        var controller = new HealthController(db);

        var result = await controller.GetDatabaseHealth(CancellationToken.None);

        var objectResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status503ServiceUnavailable);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}
