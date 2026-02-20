namespace PlasticModelApp.Infrastructure.Data.Entities;

public sealed class BrandEntity
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<PaintEntity> Paints { get; set; } = new List<PaintEntity>();
}
