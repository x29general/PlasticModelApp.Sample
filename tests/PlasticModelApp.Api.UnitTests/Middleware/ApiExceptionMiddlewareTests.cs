using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using PlasticModelApp.Api.Contracts.Responses;
using PlasticModelApp.Api.Middleware;
using PlasticModelApp.Application.Shared.Exceptions;

namespace PlasticModelApp.Api.UnitTests.Middleware;

public class ApiExceptionMiddlewareTests
{
    [Fact]
    public async Task ValidationException_ShouldMapTo400()
    {
        var middleware = new ApiExceptionMiddleware(_ => throw new ValidationException("E-400-001", "bad request", ["field error"], DateTime.UtcNow));
        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var response = await DeserializeResponse(context.Response.Body);
        response.Error.Code.Should().Be("E-400-001");
        response.Error.Status.Should().Be(400);
    }

    [Fact]
    public async Task NotFoundException_ShouldMapTo404()
    {
        var middleware = new ApiExceptionMiddleware(_ => throw new NotFoundException("E-404-001", "not found", [], DateTime.UtcNow));
        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task UnknownException_ShouldMapTo500()
    {
        var middleware = new ApiExceptionMiddleware(_ => throw new InvalidOperationException("unexpected"));
        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        var response = await DeserializeResponse(context.Response.Body);
        response.Error.Code.Should().Be("E-500-01");
    }

    private static async Task<ErrorResponse> DeserializeResponse(Stream body)
    {
        body.Position = 0;
        var response = await JsonSerializer.DeserializeAsync<ErrorResponse>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return response!;
    }
}
