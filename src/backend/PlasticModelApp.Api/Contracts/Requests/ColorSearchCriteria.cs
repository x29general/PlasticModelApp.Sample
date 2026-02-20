namespace PlasticModelApp.Api.Contracts.Requests;

public sealed class ColorSearchCriteria
{
    public int R { get; init; }

    public int G { get; init; }

    public int B { get; init; }

    public double? Threshold { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 30;
}
