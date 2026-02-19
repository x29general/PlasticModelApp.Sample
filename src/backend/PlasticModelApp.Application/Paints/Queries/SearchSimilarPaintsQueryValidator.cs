using FluentValidation;

namespace PlasticModelApp.Application.Paints.Queries;

public sealed class SearchSimilarPaintsQueryValidator : AbstractValidator<SearchSimilarPaintsQuery>
{
    public SearchSimilarPaintsQueryValidator()
    {
        RuleFor(x => x.Threshold)
            .GreaterThan(0d)
            .LessThanOrEqualTo(100d);

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
