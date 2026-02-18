using PlasticModelApp.Application.Tags.Queries;

namespace PlasticModelApp.Application.Masters.Queries;

/// <summary>
/// DTO for the result of getting all master data.
/// </summary>
public sealed record GetMastersResult
{
    /// <summary>
    /// List of brands. Each brand has an ID, name, and optional description.
    /// </summary>
    public List<GetBrandResult> Brands { get; init; } = new List<GetBrandResult>();

    /// <summary>
    /// List of glosses. Each gloss has an ID, name, and optional description.
    /// </summary>
    public List<GetGlossResult> Glosses { get; init; } = new List<GetGlossResult>();

    /// <summary>
    /// List of paint types. Each paint type has an ID, name, and optional description.
    /// </summary>
    public List<GetPaintTypeResult> PaintTypes { get; init; } = new List<GetPaintTypeResult>();

    /// <summary>
    /// List of tags. Each tag has an ID, name, optional description, and a reference to its category (ID and name). 
    /// The category information is included to avoid additional lookups when displaying tags with their categories.
    /// </summary>
    public List<GetTagByIdResult> Tags { get; init; } = new List<GetTagByIdResult>();

    /// <summary>
    /// List of tag categories. Each category has an ID, name, and optional description. 
    /// This is included to provide the necessary information for displaying tag categories without additional lookups.
    /// </summary>
    public List<GetTagCategoryResult> TagCategories { get; init; } = new List<GetTagCategoryResult>();
}

/// <summary>
/// DTO for brand information. Each brand has an ID, name, and optional description.
/// </summary>
public sealed record GetBrandResult
{
    /// <summary>
    /// Unique identifier for the brand. This is a string representation of the brand's ID in the database.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Name of the brand. This is a required field and cannot be null. It represents the display name of the brand.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Optional description of the brand. This field can be null and is intended to provide additional information about the brand.
    /// </summary>
    public string? Description { get; init; }
}

/// <summary>
/// DTO for gloss information. Each gloss has an ID, name, and optional description.
/// </summary>
public sealed record GetGlossResult
{
    /// <summary>
    /// Unique identifier for the gloss. This is a string representation of the gloss's ID in the database.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Name of the gloss. This is a required field and cannot be null. It represents the display name of the gloss.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Optional description of the gloss. This field can be null and is intended to provide additional information about the gloss.
    /// </summary>
    public string? Description { get; init; }
}

/// <summary>
/// DTO for paint type information. Each paint type has an ID, name, and optional description.
/// </summary>
public sealed record GetPaintTypeResult
{
    /// <summary>
    /// Unique identifier for the paint type. This is a string representation of the paint type's ID in the database.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Name of the paint type. This is a required field and cannot be null. It represents the display name of the paint type.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Optional description of the paint type. This field can be null and is intended to provide additional information about the paint type.
    /// </summary>
    public string? Description { get; init; }
}

/// <summary>
/// DTO for tag category information. Each tag category has an ID, name, and optional description.
/// </summary>
public sealed record GetTagCategoryResult
{
    /// <summary>
    /// Unique identifier for the tag category. This is a string representation of the tag category's ID in the database.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Name of the tag category. This is a required field and cannot be null. It represents the display name of the tag category.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Optional description of the tag category. This field can be null and is intended to provide additional information about the tag category.
    /// </summary>
    public string? Description { get; init; }
}
