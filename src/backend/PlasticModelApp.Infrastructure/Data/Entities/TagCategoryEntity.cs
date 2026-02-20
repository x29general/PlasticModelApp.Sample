namespace PlasticModelApp.Infrastructure.Data.Entities;

public sealed class TagCategoryEntity
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();
}
