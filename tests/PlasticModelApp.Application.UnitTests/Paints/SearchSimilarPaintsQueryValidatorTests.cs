using FluentAssertions;
using PlasticModelApp.Application.Paints.Queries;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Paints;

public class SearchSimilarPaintsQueryValidatorTests
{
    private readonly SearchSimilarPaintsQueryValidator _validator = new();

    [Fact]
    public void Validate_Returns_Valid_For_Default_Request()
    {
        var query = new SearchSimilarPaintsQuery(10, 20, 30);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validate_Returns_Invalid_For_Threshold(double threshold)
    {
        var query = new SearchSimilarPaintsQuery(10, 20, 30, threshold: threshold);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_Returns_Invalid_For_Page_Less_Than_One()
    {
        var query = new SearchSimilarPaintsQuery(10, 20, 30, page: 0);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Validate_Returns_Invalid_For_PageSize_Out_Of_Range(int pageSize)
    {
        var query = new SearchSimilarPaintsQuery(10, 20, 30, pageSize: pageSize);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
    }
}
