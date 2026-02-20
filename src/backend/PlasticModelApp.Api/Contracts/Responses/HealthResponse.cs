namespace PlasticModelApp.Api.Contracts.Responses;

public sealed class HealthResponse
{
    public string Status { get; init; } = "ok";
}

public sealed class DatabaseHealthResponse
{
    public string status { get; init; } = "ok";

    public string? message { get; init; }
}
