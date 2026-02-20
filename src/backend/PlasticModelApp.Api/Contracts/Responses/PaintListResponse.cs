namespace PlasticModelApp.Api.Contracts.Responses;

public sealed class PaintListResponse
{
    public List<PaintListItem> Items { get; init; } = [];

    public int Total { get; init; }

    public int Page { get; init; }

    public int PageSize { get; init; }
}

public sealed class PaintListItem
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string ModelNumber { get; init; } = string.Empty;

    public string Brand { get; init; } = string.Empty;

    public string? PaintType { get; init; }

    public string? Gloss { get; init; }

    public string Hex { get; init; } = string.Empty;
}
