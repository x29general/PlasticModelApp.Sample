namespace PlasticModelApp.Infrastructure.Data.Entities;

public sealed class TagEntity
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string TagCategoryId { get; set; } = null!;

    public string? Hex { get; set; }

    public string? Effect { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public TagCategoryEntity? TagCategory { get; set; }

    public ICollection<PaintTagEntity> PaintTags { get; set; } = new List<PaintTagEntity>();
}
