namespace PlasticModelApp.Api.Contracts.Requests;

public sealed class PaintSearchCriteria
{
    public List<string> BrandIds { get; init; } = [];

    public List<string> PaintTypeIds { get; init; } = [];

    public List<string> GlossIds { get; init; } = [];

    public List<string> TagIds { get; init; } = [];

    public string Sort { get; init; } = "ModelNumberAsc";

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 30;
}
