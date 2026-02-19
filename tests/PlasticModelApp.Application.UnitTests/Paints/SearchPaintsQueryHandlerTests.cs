using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Application.Shared.Exceptions;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Paints;

public class SearchPaintsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Normalizes_Criteria_And_Returns_Result()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<SearchPaintsQueryHandler>>();
        SearchPaintsQuery? captured = null;
        var expected = new SearchPaintsResult
        {
            Total = 1,
            Items = new List<SearchPaintsResultItem> { new() { Id = "P1", Name = "Paint" } }
        };

        svc.Setup(x => x.SearchAsync(It.IsAny<SearchPaintsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<SearchPaintsQuery, CancellationToken>((q, _) => captured = q)
            .ReturnsAsync(expected);

        var criteria = new Criteria
        {
            Page = 0,
            PageSize = 0,
            BrandIds = null!,
            PaintTypeIds = null!,
            GlossIds = null!,
            TagIds = null!
        };

        var handler = new SearchPaintsQueryHandler(svc.Object, logger.Object);
        var result = await handler.Handle(new SearchPaintsQuery(criteria), CancellationToken.None);

        result.Should().BeSameAs(expected);
        captured.Should().NotBeNull();
        captured!.Criteria.Page.Should().Be(1);
        captured.Criteria.PageSize.Should().Be(20);
        captured.Criteria.BrandIds.Should().NotBeNull();
        captured.Criteria.PaintTypeIds.Should().NotBeNull();
        captured.Criteria.GlossIds.Should().NotBeNull();
        captured.Criteria.TagIds.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_Throws_AppSystemException_When_Service_Fails()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<SearchPaintsQueryHandler>>();
        svc.Setup(x => x.SearchAsync(It.IsAny<SearchPaintsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("db down"));

        var handler = new SearchPaintsQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<AppSystemException>(() =>
            handler.Handle(new SearchPaintsQuery(new Criteria()), CancellationToken.None));
    }
}
