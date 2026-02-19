using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PlasticModelApp.Application.Shared.Exceptions;
using PlasticModelApp.Application.Tags.Interfaces;
using PlasticModelApp.Application.Tags.Queries;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Tags;

public class GetTagHandlersTests
{
    [Fact]
    public async Task GetTagById_Returns_When_Found()
    {
        var svc = new Mock<ITagQueryService>();
        var logger = new Mock<ILogger<GetTagByIdQueryHandler>>();
        svc.Setup(s => s.GetByIdAsync(It.IsAny<PlasticModelApp.Domain.Catalog.ValueObjects.TagId>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new GetTagByIdResult { Id = "T1", Name = "N", CategoryId = "TC1" });

        var handler = new GetTagByIdQueryHandler(svc.Object, logger.Object);
        var dto = await handler.Handle(new GetTagByIdQuery("T1"), CancellationToken.None);
        dto.Should().BeEquivalentTo(new { Id = "T1", Name = "N", CategoryId = "TC1" });
    }

    [Fact]
    public async Task GetTagById_Throws_When_NotFound_Or_Deleted()
    {
        var svc = new Mock<ITagQueryService>();
        var logger = new Mock<ILogger<GetTagByIdQueryHandler>>();
        svc.SetupSequence(s => s.GetByIdAsync(It.IsAny<PlasticModelApp.Domain.Catalog.ValueObjects.TagId>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync((GetTagByIdResult?)null)
           .ReturnsAsync(new GetTagByIdResult { Id = "T2", IsDeleted = true, Name = "N", CategoryId = "TC1" });

        var handler = new GetTagByIdQueryHandler(svc.Object, logger.Object);
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetTagByIdQuery("X"), CancellationToken.None));
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(new GetTagByIdQuery("T2"), CancellationToken.None));
    }

    [Fact]
    public async Task GetTags_Returns_Items_And_Total()
    {
        var svc = new Mock<ITagQueryService>();
        var logger = new Mock<ILogger<GetTagsQueryHandler>>();
        var list = new List<GetTagByIdResult> { new() { Id = "T1", Name = "A", CategoryId = "TC1" } };
        svc.Setup(s => s.GetByCategoryAsync("Cat", It.IsAny<CancellationToken>())).ReturnsAsync(list);

        var handler = new GetTagsQueryHandler(svc.Object, logger.Object);
        var res = await handler.Handle(new GetTagsQuery("Cat"), CancellationToken.None);
        res.Total.Should().Be(1);
        res.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetTagById_Rethrows_Unexpected_Exception()
    {
        var svc = new Mock<ITagQueryService>();
        var logger = new Mock<ILogger<GetTagByIdQueryHandler>>();
        svc.Setup(s => s.GetByIdAsync(It.IsAny<PlasticModelApp.Domain.Catalog.ValueObjects.TagId>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new GetTagByIdQueryHandler(svc.Object, logger.Object);
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetTagByIdQuery("T1"), CancellationToken.None));
    }

    [Fact]
    public async Task GetTags_Rethrows_Unexpected_Exception()
    {
        var svc = new Mock<ITagQueryService>();
        var logger = new Mock<ILogger<GetTagsQueryHandler>>();
        svc.Setup(s => s.GetByCategoryAsync(It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var handler = new GetTagsQueryHandler(svc.Object, logger.Object);
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new GetTagsQuery("Cat"), CancellationToken.None));
    }
}
