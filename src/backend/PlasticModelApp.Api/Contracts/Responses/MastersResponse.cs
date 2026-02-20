namespace PlasticModelApp.Api.Contracts.Responses;

public sealed class MastersResponse
{
    public List<MasterItemResponse> Brands { get; init; } = [];

    public List<MasterItemResponse> PaintTypes { get; init; } = [];

    public List<MasterItemResponse> Glosses { get; init; } = [];

    public List<TagResponse> Tags { get; init; } = [];

    public List<MasterItemResponse> TagCategories { get; init; } = [];
}

public sealed class MasterItemResponse
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
}

public sealed class TagResponse
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public string? Hex { get; init; }

    public string? Effect { get; init; }

    public string? Description { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
