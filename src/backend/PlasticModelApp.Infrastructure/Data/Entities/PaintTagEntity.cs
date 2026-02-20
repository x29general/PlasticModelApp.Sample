namespace PlasticModelApp.Infrastructure.Data.Entities;

public sealed class PaintTagEntity
{
    public string PaintId { get; set; } = null!;

    public string TagId { get; set; } = null!;

    public PaintEntity? Paint { get; set; }

    public TagEntity? Tag { get; set; }
}
