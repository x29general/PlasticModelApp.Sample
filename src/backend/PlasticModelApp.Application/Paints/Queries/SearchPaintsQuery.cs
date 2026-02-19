using MediatR;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Query to get a list of paints based on specified criteria.
/// </summary>
public sealed class SearchPaintsQuery : IRequest<SearchPaintsResult>
{
    /// <summary>
    /// Criteria for searching paints, including filters and sorting options.
    /// </summary>
    public Criteria Criteria { get; }

    /// <summary>
    /// Initializes a new instance of the `SearchPaintsQuery` class with the specified search criteria.
    /// </summary>
    /// <param name="criteria">The criteria for searching paints, which includes various filters and sorting options.</param>
    public SearchPaintsQuery(Criteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        Criteria = criteria;
    }
}

/// <summary>
/// Criteria for searching paints, including filters and sorting options.
/// </summary>
public sealed class Criteria
{
    /// <summary>
    /// List of brand IDs to filter the paints by their associated brands.
    /// </summary>
    public List<string> BrandIds { get; set; } = new();

    /// <summary>
    /// List of paint type IDs to filter the paints by their associated paint types.
    /// </summary>
    public List<string> PaintTypeIds { get; set; } = new();

    /// <summary>
    /// List of gloss level IDs to filter the paints by their associated gloss levels.
     /// </summary>
    public List<string> GlossIds { get; set; } = new();

    /// <summary>
    /// List of tag IDs to filter the paints by their associated tags.
    /// </summary>
    public List<string> TagIds { get; set; } = new();

    /// <summary>
    /// Sorting option for the search results, which determines the order in which the paints are returned based on specific attributes such as name, model number.
    /// </summary>
    /// <remarks> The default sorting option is by model number in ascending order. </remarks>
    public SearchPaintsSortOption Sort { get; set; } = SearchPaintsSortOption.ModelNumberAsc;

    /// <summary>
    /// Page number for pagination, which indicates the specific page of results to retrieve when the search results are paginated.
    /// The default value is 1, which means the first page of results will be returned if pagination is applied.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page for pagination, which determines how many paint records are included in each page of results when the search results are paginated.
    /// </summary>
    public int PageSize { get; set; } = 30;

    /// <summary>
    /// Normalizes the criteria by ensuring that pagination parameters are set to valid values and that filter lists are initialized to empty lists if they are null.
    /// This method should be called before executing the search to ensure that the criteria are in a consistent state.
    /// </summary>
    public void Normalize()
    {
        Page = Page <= 0 ? 1 : Page;
        PageSize = PageSize <= 0 ? 20 : PageSize;
        BrandIds ??= new();
        PaintTypeIds ??= new();
        GlossIds ??= new();
        TagIds ??= new();
    }
}

