using MediatR;
using PlasticModelApp.Domain.Catalog.ValueObjects;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Query for searching paints similar in color to a specified RGB color, with an optional threshold for color similarity and pagination parameters.
/// The query returns a list of paints that are similar in color to the specified RGB color based on the CIEDE2000 color difference formula, along with pagination information for the results.
/// </summary>
public sealed class SearchSimilarPaintsQuery : IRequest<SimilarPaintsResult>
{
    private const double DefaultThreshold = 5d;
    private const int DefaultPageSize = 30;
    private const int MaxPageSize = 100;

    /// <summary>
    /// Initializes a new instance of the `SearchSimilarPaintsQuery` class.
    /// </summary>
    /// <param name="r">Red component of the target RGB color (0-255).</param>
    /// <param name="g">Green component of the target RGB color (0-255).</param>
    /// <param name="b">Blue component of the target RGB color (0-255).</param>
    /// <param name="threshold">Optional threshold for color similarity based on CIEDE2000 distance. A smaller value indicates a closer match. Default is 5.</param>
    /// <param name="page">Page number for pagination. Default is 1.</param>
    /// <param name="pageSize">Number of results per page for pagination. Default is 30, and the maximum allowed is 100.</param>
    public SearchSimilarPaintsQuery(int r, int g, int b, double? threshold = null, int page = 1, int pageSize = DefaultPageSize)
    {
        TargetColor = new RgbColor(r, g, b);
        Threshold = threshold ?? DefaultThreshold;
        Page = page;
        PageSize = pageSize;
    }

    /// <summary>
    /// Selected RGB color for which similar paints will be searched. 
    /// </summary>
    public RgbColor TargetColor { get; }

    /// <summary>
    /// Threshold for color similarity based on CIEDE2000 distance. A smaller value indicates a closer match. Default is 5.
    /// </summary>
    public double Threshold { get; private set; }

    /// <summary>
    /// Page number for pagination. Default is 1. If the value is less than or equal to 0, it will be set to 1.
    /// </summary>
    public int Page { get; private set; }

    /// <summary>
    /// Number of results per page for pagination. Default is 30, and the maximum allowed is 100. 
    /// If the value is less than or equal to 0, it will be set to the default of 30. If the value exceeds the maximum of 100, it will be set to 100.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Normalizes the pagination parameters and threshold to ensure they are within valid ranges.
    /// </summary>
    internal void Normalize()
    {
        Page = Page <= 0 ? 1 : Page;
        PageSize = PageSize <= 0 ? DefaultPageSize : PageSize;
        PageSize = PageSize > MaxPageSize ? MaxPageSize : PageSize;
        Threshold = Threshold <= 0 ? DefaultThreshold : Math.Min(Threshold, 100);
    }
}
