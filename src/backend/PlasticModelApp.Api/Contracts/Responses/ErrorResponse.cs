namespace PlasticModelApp.Api.Contracts.Responses;

public sealed class ErrorResponse
{
    public ErrorBody Error { get; init; } = new();
}

public sealed class ErrorBody
{
    public int Status { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public List<ErrorDetail> Details { get; init; } = [];

    public DateTime Timestamp { get; init; }
}

public sealed class ErrorDetail
{
    public string? Field { get; init; }

    public string Issue { get; init; } = string.Empty;
}
