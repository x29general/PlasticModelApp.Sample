using PlasticModelApp.Application.Tags.Queries;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Result of `GetPaintByIdQuery`.
/// </summary>
public sealed record class GetPaintByIdResult
{
    /// <summary>
    /// Unique identifier of the paint.
    /// </summary>
    public string Id { get; init; } = null!;

    /// <summary>
    /// Name of the paint.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Model number of the paint, which is often used to identify the specific color or type of paint within a brand's product line.
    /// </summary>
    public string ModelNumber { get; init; } = null!;

    /// <summary>
    /// Identifier of the brand that produces the paint. 
    /// This is a reference to the brand's unique identifier, which can be used to retrieve more information about the brand if needed.
    /// </summary>
    public string BrandId { get; init; } = null!;

    /// <summary>
    /// Name of the brand that produces the paint. 
    /// This is the human-readable name of the brand, which can be displayed in the user interface or used in other contexts where the brand's name is needed.
    /// </summary>
    public string BrandName { get; init; } = null!;

    /// <summary>
    /// Identifier of the paint type, which categorizes the paint based on its intended use or characteristics (e.g., acrylic, enamel, lacquer).
    /// This is a reference to the paint type's unique identifier, which can be used to retrieve more information about the paint type if needed.
    /// </summary>
    public string PaintTypeId { get; init; } = null!;

    /// <summary>
    /// Name of the paint type, which categorizes the paint based on its intended use or characteristics (e.g., acrylic, enamel, lacquer).
    /// This is the human-readable name of the paint type, which can be displayed in the user interface or used in other contexts where the paint type's name is needed.
    /// </summary>
    public string PaintTypeName { get; init; } = null!;

    /// <summary>
    /// Identifier of the gloss level, which describes the finish of the paint (e.g., matte, gloss).
    /// This is a reference to the gloss level's unique identifier, which can be used to retrieve more information about the gloss level if needed.
    /// </summary>
    public string GlossId { get; init; } = null!;

    /// <summary>
    /// Name of the gloss level, which describes the finish of the paint (e.g., matte, gloss).
    /// This is the human-readable name of the gloss level, which can be displayed in the user interface or used in other contexts where the gloss level's name is needed.
    /// </summary>
    public string GlossName { get; init; } = null!;

    /// <summary>
    /// Price of the paint, which represents the cost of the paint. 
    /// This value is typically used for display purposes and may be used in calculations related to pricing or budgeting for painting projects.
    /// The price is represented as a decimal value, which allows for accurate representation of currency values, including fractional amounts.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Description of the paint, which provides additional information about the paint, such as its characteristics, recommended uses, 
    /// or any other relevant details that may be helpful for users when selecting a paint.
    /// This field is optional and may be null if no description is provided for the paint.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Hexadecimal color code of the paint, which represents the color of the paint in a standard format that can be used in digital applications.
    /// </summary>
    public string Hex { get; init; } = null!;

    /// <summary>
    /// value of the red component in the RGB color model, which represents the intensity of the red color in the paint.
    /// </summary>
    public int Rgb_R { get; init; }

    /// <summary>
    /// Value of the green component in the RGB color model, which represents the intensity of the green color in the paint.
    /// </summary>
    public int Rgb_G { get; init; }

    /// <summary>
    /// Value of the blue component in the RGB color model, which represents the intensity of the blue color in the paint.
    /// </summary>
    public int Rgb_B { get; init; }

    /// <summary>
    /// Value of the hue component in the HSL color model, which represents the type of color (e.g., red, green, blue) in the paint.
    /// </summary>
    public float Hsl_H { get; init; }

    /// <summary>
    /// Value of the saturation component in the HSL color model, which represents the intensity or purity of the color in the paint.
    /// </summary>
    public float Hsl_S { get; init; }

    /// <summary>
    /// Value of the lightness component in the HSL color model, which represents the brightness or darkness of the color in the paint.
    /// </summary>
    public float Hsl_L { get; init; }

    /// <summary>
    /// URL of the paint's image, which can be used to display a visual representation of the paint in the user interface.
    /// This field is optional and may be null if no image is available for the paint.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// List of tags associated with the paint, which provides additional categorization or labeling for the paint.
    /// </summary>
    public List<GetTagByIdResult> Tags { get; init; } = new();

    /// <summary>
    /// Date and time when the paint was created. 
    /// This timestamp is typically used for record-keeping and may be displayed in the user interface to indicate when the paint was added to the system.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Date and time when the paint was last updated. 
    /// This timestamp is typically used for record-keeping and may be displayed in the user interface to indicate when the paint information was last modified.
    /// </summary>
    public DateTime UpdatedAt { get; init; }

    /// <summary>
    /// Indicates whether the paint is marked as deleted.
    /// This field is used to soft-delete paints, allowing them to be hidden from users without permanently removing them from the database.
    /// When a paint is marked as deleted, it should not be returned in query results for active paints, but it may still exist in the database for record-keeping or auditing purposes.
    /// </summary>
    public bool IsDeleted { get; init; }
}

