using FluentAssertions;
using Moq;
using PlasticModelApp.Application.Masters.Interfaces;
using PlasticModelApp.Application.Masters.Queries;
using Microsoft.Extensions.Logging;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Masters;

public class GetMastersQueryHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Service_Result()
    {
        var svc = new Mock<IMasterQueryService>();
        var logger = new Mock<ILogger<GetMastersQueryHandler>>();
        var expected = new GetMastersResult { Brands = new(), Glosses = new(), PaintTypes = new(), TagCategories = new(), Tags = new() };
        svc.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var handler = new GetMastersQueryHandler(svc.Object, logger.Object);
        var res = await handler.Handle(new GetMastersQuery(), CancellationToken.None);

        res.Should().BeSameAs(expected);
        svc.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Rethrows_When_Service_Throws()
    {
        var svc = new Mock<IMasterQueryService>();
        var logger = new Mock<ILogger<GetMastersQueryHandler>>();
        svc.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new GetMastersQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetMastersQuery(), CancellationToken.None));
    }
}

