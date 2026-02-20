namespace PlasticModelApp.Api.Contracts.Responses;

public sealed class PaintResponse
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string ModelNumber { get; init; } = string.Empty;

    public string Brand { get; init; } = string.Empty;

    public string PaintType { get; init; } = string.Empty;

    public string Gloss { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string? Description { get; init; }

    public string Hex { get; init; } = string.Empty;

    public int RgbR { get; init; }

    public int RgbG { get; init; }

    public int RgbB { get; init; }

    public float HslH { get; init; }

    public float HslS { get; init; }

    public float HslL { get; init; }

    public string? ImageUrl { get; init; }

    public List<TagResponse> Tags { get; init; } = [];

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
