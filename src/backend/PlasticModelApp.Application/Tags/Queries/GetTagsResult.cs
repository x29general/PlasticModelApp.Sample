namespace PlasticModelApp.Application.Tags.Queries;

/// <summary>
/// DTO representing the result of a `GetTagsQuery`. 
/// This class contains a list of tags that match the specified category filter and the total count of those tags. 
/// </summary>
public class GetTagsResult
{
    /// <summary>
    /// A list of tags that match the specified category filter. 
    /// Each item in the list is an instance of `GetTagByIdResult`, which contains detailed information about a single tag.
    /// This list is initialized as an empty list to ensure it is never null, even if no tags match the criteria.
    /// </summary>
    public List<GetTagByIdResult> Items { get; set; } = new List<GetTagByIdResult>();

    /// <summary>
    /// The total count of tags that match the specified category filter. 
    /// This is an integer value that represents how many tags are included in the `Items` list. 
    /// It is set to zero by default and is updated to reflect the actual count of matching tags when the query is processed.
    /// </summary>
    public int Total { get; set; }
}
