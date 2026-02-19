namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// DTO for search similar paints result, which contains a list of similar paint details and the total count of similar paints matching the search criteria.
/// </summary>
public sealed class SimilarPaintsResult
{
    /// <summary>
    /// List of similar paint details in the search results.
    /// </summary>
    public List<SimilarSearchPaintResultItem> Items { get; init; } = new();

    /// <summary>
    /// Total count of similar paints matching the search criteria.
    /// </summary>
    public int Total { get; init; }
}

/// <summary>
/// DTO for individual paint details in the similar paint search results, which extends SearchPaintsResultItem and includes an additional property for similarity score.
/// </summary>
public sealed class SimilarSearchPaintResultItem : SearchPaintsResultItem
{
    /// <summary>
    /// Similarity score between the original paint and the similar paint.
    /// </summary>
    public double Similarity { get; init; }
}
