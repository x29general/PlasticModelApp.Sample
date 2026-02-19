using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PlasticModelApp.Application.Paints.Interfaces;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Application.Shared.Exceptions;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Paints;

public class GetPaintByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Paint_When_Found()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<GetPaintByIdQueryHandler>>();
        svc.Setup(x => x.GetByIdAsync(It.IsAny<PaintId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPaintByIdResult
            {
                Id = "P1",
                Name = "Paint",
                ModelNumber = "C1",
                BrandId = "B1",
                BrandName = "Brand",
                PaintTypeId = "PT1",
                PaintTypeName = "Type",
                GlossId = "G1",
                GlossName = "Gloss",
                Hex = "#112233"
            });

        var handler = new GetPaintByIdQueryHandler(svc.Object, logger.Object);
        var result = await handler.Handle(new GetPaintByIdQuery("P1"), CancellationToken.None);

        result.Id.Should().Be("P1");
        result.Name.Should().Be("Paint");
    }

    [Fact]
    public async Task Handle_Throws_NotFound_When_Result_Is_Null()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<GetPaintByIdQueryHandler>>();
        svc.Setup(x => x.GetByIdAsync(It.IsAny<PaintId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetPaintByIdResult?)null);

        var handler = new GetPaintByIdQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetPaintByIdQuery("P1"), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Throws_NotFound_When_Result_Is_Deleted()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<GetPaintByIdQueryHandler>>();
        svc.Setup(x => x.GetByIdAsync(It.IsAny<PaintId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPaintByIdResult
            {
                Id = "P1",
                Name = "Deleted",
                ModelNumber = "C1",
                BrandId = "B1",
                BrandName = "Brand",
                PaintTypeId = "PT1",
                PaintTypeName = "Type",
                GlossId = "G1",
                GlossName = "Gloss",
                Hex = "#112233",
                IsDeleted = true
            });

        var handler = new GetPaintByIdQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetPaintByIdQuery("P1"), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Rethrows_Unexpected_Exception()
    {
        var svc = new Mock<IPaintQueryService>();
        var logger = new Mock<ILogger<GetPaintByIdQueryHandler>>();
        svc.Setup(x => x.GetByIdAsync(It.IsAny<PaintId>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new GetPaintByIdQueryHandler(svc.Object, logger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetPaintByIdQuery("P1"), CancellationToken.None));
    }
}
