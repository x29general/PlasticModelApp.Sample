namespace PlasticModelApp.Application.Tags.Queries;

/// <summary>
/// DTO for tag information. 
/// This class represents the data structure for a tag, including its ID, name, 
/// description, color (hex), effect, category ID, creation and update timestamps, and deletion status.
/// </summary>
public class GetTagByIdResult
{
    /// <summary>
    /// Unique identifier for the tag. This is a string representation of the tag's ID in the database. It is a required field and cannot be null.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// Name of the tag. This is a required field and cannot be null. It represents the display name of the tag.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Optional description of the tag. This field can be null and is intended to provide additional information about the tag.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional hexadecimal color code for the tag. This field can be null and is intended to provide a visual representation of the tag's color.
    /// </summary>
    public string? Hex { get; set; }

    /// <summary>
    /// Optional effect description for the tag. This field can be null and is intended to provide additional information about the tag's effect or purpose.
    /// </summary>
    public string? Effect { get; set; }

    /// <summary>
    /// Category ID to which the tag belongs. This is a required field and cannot be null. It represents the identifier of the category that the tag is associated with.
    /// </summary>
    public string CategoryId { get; set; } = null!;

    /// <summary>
    /// Timestamp of when the tag was created. This is a required field and cannot be null. It represents the date and time when the tag was initially created in the system.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp of when the tag was last updated. This is a required field and cannot be null. It represents the date and time when the tag was last modified in the system.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Indicates whether the tag is marked as deleted. This is a required field and cannot be null. 
    /// It represents the deletion status of the tag, where `true` means the tag is deleted and `false` means it is active.
    /// </summary>
    public bool IsDeleted { get; set; }
}
