using System.Net;
using System.Text.Json;
using PlasticModelApp.Api.Contracts.Responses;
using PlasticModelApp.Application.Shared.Exceptions;

namespace PlasticModelApp.Api.Middleware;

public sealed class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteErrorResponseAsync(context, ex);
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code, message, details, timestamp) = exception switch
        {
            ValidationException validation => (
                (int)HttpStatusCode.BadRequest,
                validation.Code,
                validation.Message,
                validation.Details,
                validation.Timestamp),
            NotFoundException notFound => (
                (int)HttpStatusCode.NotFound,
                notFound.Code,
                notFound.Message,
                notFound.Details,
                notFound.Timestamp),
            ForbiddenException forbidden => (
                (int)HttpStatusCode.Forbidden,
                forbidden.Code,
                forbidden.Message,
                forbidden.Details,
                forbidden.Timestamp),
            UnauthorizedException unauthorized => (
                (int)HttpStatusCode.Unauthorized,
                unauthorized.Code,
                unauthorized.Message,
                unauthorized.Details,
                unauthorized.Timestamp),
            AppSystemException appSystem => (
                (int)HttpStatusCode.InternalServerError,
                appSystem.Code,
                appSystem.Message,
                appSystem.Details,
                appSystem.Timestamp),
            _ => (
                (int)HttpStatusCode.InternalServerError,
                "E-500-01",
                "サーバー内部エラーが発生しました",
                Array.Empty<object>(),
                DateTime.UtcNow)
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Error = new ErrorBody
            {
                Status = statusCode,
                Code = code,
                Message = message,
                Details = MapDetails(details),
                Timestamp = timestamp
            }
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = null
        });

        await context.Response.WriteAsync(json);
    }

    private static List<ErrorDetail> MapDetails(object[] details)
    {
        return details
            .Select(d => new ErrorDetail
            {
                Field = null,
                Issue = d?.ToString() ?? string.Empty
            })
            .ToList();
    }
}
