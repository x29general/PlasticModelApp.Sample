namespace PlasticModelApp.Infrastructure.Data.Entities;

public sealed class PaintEntity
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ModelNumber { get; set; } = null!;

    public string? ModelNumberPrefix { get; set; }

    public int? ModelNumberNumber { get; set; }

    public string BrandId { get; set; } = null!;

    public string PaintTypeId { get; set; } = null!;

    public string GlossId { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string Hex { get; set; } = null!;

    public int RgbR { get; set; }

    public int RgbG { get; set; }

    public int RgbB { get; set; }

    public float HslH { get; set; }

    public float HslS { get; set; }

    public float HslL { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public BrandEntity? Brand { get; set; }

    public PaintTypeEntity? PaintType { get; set; }

    public GlossEntity? Gloss { get; set; }

    public ICollection<PaintTagEntity> PaintTags { get; set; } = new List<PaintTagEntity>();
}
