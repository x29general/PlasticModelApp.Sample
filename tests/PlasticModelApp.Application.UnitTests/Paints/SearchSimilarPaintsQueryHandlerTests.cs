using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Application.Shared.Exceptions;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Paints;

public class SearchSimilarPaintsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Normalizes_Query_And_Returns_Result()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<SearchSimilarPaintsQueryHandler>>();
        SearchSimilarPaintsQuery? captured = null;
        var expected = new SimilarPaintsResult { Total = 1 };

        svc.Setup(x => x.SearchSimilarAsync(It.IsAny<SearchSimilarPaintsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<SearchSimilarPaintsQuery, CancellationToken>((q, _) => captured = q)
            .ReturnsAsync(expected);

        var handler = new SearchSimilarPaintsQueryHandler(svc.Object, logger.Object);
        var result = await handler.Handle(new SearchSimilarPaintsQuery(1, 2, 3, threshold: -1, page: 0, pageSize: 200), CancellationToken.None);

        result.Should().BeSameAs(expected);
        captured.Should().NotBeNull();
        captured!.Threshold.Should().Be(5d);
        captured.Page.Should().Be(1);
        captured.PageSize.Should().Be(100);
    }

    [Fact]
    public async Task Handle_Throws_ValidationException_When_Request_Is_Null()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<SearchSimilarPaintsQueryHandler>>();
        var handler = new SearchSimilarPaintsQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(null!, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Rethrows_ValidationException_From_Service()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<SearchSimilarPaintsQueryHandler>>();
        svc.Setup(x => x.SearchSimilarAsync(It.IsAny<SearchSimilarPaintsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException("E-400-001", "bad request", Array.Empty<object>(), DateTime.UtcNow));

        var handler = new SearchSimilarPaintsQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(new SearchSimilarPaintsQuery(1, 2, 3), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Throws_AppSystemException_When_Service_Fails()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<SearchSimilarPaintsQueryHandler>>();
        svc.Setup(x => x.SearchSimilarAsync(It.IsAny<SearchSimilarPaintsQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("db down"));

        var handler = new SearchSimilarPaintsQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<AppSystemException>(() =>
            handler.Handle(new SearchSimilarPaintsQuery(1, 2, 3), CancellationToken.None));
    }
}
