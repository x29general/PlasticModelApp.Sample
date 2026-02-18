using MediatR;

namespace PlasticModelApp.Application.Tags.Queries;

/// <summary>
/// Query to get a list of tags based on an optional category filter. 
/// If the category is provided, it returns tags that belong to that category; otherwise, it returns all tags. 
/// The result includes a list of tag information matching the specified criteria.
/// </summary>
public class GetTagsQuery : IRequest<GetTagsResult>
{
    /// <summary>
    /// Optional category filter for retrieving tags. If this property is set, the query will return only tags that belong to the specified category.
    /// </summary>
    public string? Category { get; }

    /// <summary>
    /// Initializes a new instance of the `GetTagsQuery` class with an optional category filter. 
    /// If the category is not provided, the query will return all tags.
    /// </summary>
    /// <param name="category">The category to filter tags by (optional).</param>
    public GetTagsQuery(string? category = null)
    {
        Category = category;
    }
}
