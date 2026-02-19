namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// DTO for search paints result, which contains a list of paint details and the total count of paints matching the search criteria.
/// </summary>
public class SearchPaintsResult
{
    /// <summary>
    /// List of paint details in the search results.
    /// </summary>
    public List<SearchPaintsResultItem> Items { get; set; } = new();

    /// <summary>
    /// Total count of paints matching the search criteria.
    /// </summary>
    public int Total { get; set; }
}

/// <summary>
/// DTO for individual paint details in the search results, which includes properties such as ID, name, model number, brand name, and hex color code of the paint.
/// </summary>
public class SearchPaintsResultItem
{
    /// <summary>
    /// Unique identifier for the paint.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the paint.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Model number of the paint, which is often used to identify the specific color or type of paint within a brand's product line.
    /// </summary>
    public string ModelNumber { get; set; } = string.Empty;

    /// <summary>
    /// Name of the brand that produces the paint. 
    /// </summary>
    public string BrandName { get; set; } = string.Empty;

    /// <summary>
    /// Hexadecimal color code representing the color of the paint, which can be used for display purposes or to find similar colors.
    /// </summary>
    public string HexColor { get; set; } = string.Empty;
}