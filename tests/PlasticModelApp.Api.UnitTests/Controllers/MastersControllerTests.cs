using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlasticModelApp.Api.Controllers;
using PlasticModelApp.Application.Masters.Queries;
using PlasticModelApp.Application.Tags.Queries;

namespace PlasticModelApp.Api.UnitTests.Controllers;

public class MastersControllerTests
{
    [Fact]
    public async Task GetMasters_ShouldReturnMappedResponse()
    {
        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<GetMastersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetMastersResult
            {
                Brands = [new GetBrandResult { Id = "b1", Name = "Brand1" }],
                PaintTypes = [new GetPaintTypeResult { Id = "pt1", Name = "Acrylic" }],
                Glosses = [new GetGlossResult { Id = "g1", Name = "Gloss" }],
                TagCategories = [new GetTagCategoryResult { Id = "tc1", Name = "Finish" }],
                Tags = [new GetTagByIdResult { Id = "t1", Name = "Metallic", CategoryId = "tc1" }]
            });

        var controller = new MastersController(sender.Object);

        var result = await controller.GetMasters(CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value.Should().BeAssignableTo<PlasticModelApp.Api.Contracts.Responses.MastersResponse>().Subject;
        response.Brands.Should().ContainSingle(x => x.Id == "b1" && x.Name == "Brand1");
        response.Tags.Should().ContainSingle(x => x.Id == "t1" && x.Category == "tc1");
    }
}
