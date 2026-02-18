using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// PaintTag represents the association between Paint and Tag.
/// </summary>
public class PaintTag
{
    public PaintId PaintId { get; private set; } = null!;

    public TagId TagId { get; private set; } = null!;
 
    protected PaintTag() { } // EF Core

    private PaintTag(PaintId paintId, TagId tagId)
    {
        ArgumentNullException.ThrowIfNull(paintId);
        ArgumentNullException.ThrowIfNull(tagId);

        PaintId = paintId;
        TagId = tagId;
    }

    /// <summary>
    /// Creates a new instance of the PaintTag class.
    /// </summary>
    /// <param name="paintId">Paint ID</param>
    /// <param name="tagId">Tag ID</param>
    public static PaintTag Create(PaintId paintId, TagId tagId) => new(paintId, tagId);
}
 
