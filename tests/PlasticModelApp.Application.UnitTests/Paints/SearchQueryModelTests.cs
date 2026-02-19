using FluentAssertions;
using PlasticModelApp.Application.Paints.Queries;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Paints;

public class SearchQueryModelTests
{
    [Fact]
    public void SearchPaintsQuery_Ctor_Throws_When_Criteria_Is_Null()
    {
        Action act = () => _ = new SearchPaintsQuery(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SearchPaintsQuery_Ctor_Assigns_Criteria()
    {
        var criteria = new Criteria();

        var query = new SearchPaintsQuery(criteria);

        query.Criteria.Should().BeSameAs(criteria);
    }

    [Fact]
    public void SearchSimilarPaintsQuery_Ctor_Uses_Default_Threshold_When_Null()
    {
        var query = new SearchSimilarPaintsQuery(1, 2, 3, threshold: null);

        query.Threshold.Should().Be(5d);
    }
}
