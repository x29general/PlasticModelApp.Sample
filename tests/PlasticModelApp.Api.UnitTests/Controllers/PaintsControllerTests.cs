using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PlasticModelApp.Api.Contracts.Requests;
using PlasticModelApp.Api.Contracts.Responses;
using PlasticModelApp.Api.Controllers;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Application.Tags.Queries;

namespace PlasticModelApp.Api.UnitTests.Controllers;

public class PaintsControllerTests
{
    [Fact]
    public async Task Search_ShouldNormalizePagingAndReturnResponse()
    {
        SearchPaintsQuery? capturedQuery = null;
        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<SearchPaintsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<SearchPaintsResult>, CancellationToken>((request, _) => capturedQuery = (SearchPaintsQuery)request)
            .ReturnsAsync(new SearchPaintsResult
            {
                Items = [new SearchPaintsResultItem { Id = "p1", Name = "Red", ModelNumber = "C1", BrandName = "Mr.Color", HexColor = "#ff0000" }],
                Total = 1,
            });

        var controller = new PaintsController(sender.Object);
        var request = new PaintSearchCriteria { Page = 0, PageSize = 0 };

        var result = await controller.Search(request, CancellationToken.None);

        capturedQuery.Should().NotBeNull();
        capturedQuery!.Criteria.Page.Should().Be(1);
        capturedQuery.Criteria.PageSize.Should().Be(20);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value.Should().BeAssignableTo<PaintListResponse>().Subject;
        response.Page.Should().Be(1);
        response.PageSize.Should().Be(20);
        response.Total.Should().Be(1);
        response.Items.Should().ContainSingle(x => x.Id == "p1" && x.Brand == "Mr.Color" && x.Hex == "#ff0000");
    }

    [Fact]
    public async Task SearchByColor_ShouldReturnMappedResponse()
    {
        SearchSimilarPaintsQuery? capturedQuery = null;
        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<SearchSimilarPaintsQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<SimilarPaintsResult>, CancellationToken>((request, _) => capturedQuery = (SearchSimilarPaintsQuery)request)
            .ReturnsAsync(new SimilarPaintsResult
            {
                Items = [new SimilarSearchPaintResultItem { Id = "p2", Name = "Blue", ModelNumber = "C2", BrandName = "Gaia", HexColor = "#0000ff", Similarity = 2.1 }],
                Total = 1,
            });

        var controller = new PaintsController(sender.Object);

        var result = await controller.SearchByColor(new ColorSearchCriteria { R = 10, G = 20, B = 30, Page = 0, PageSize = 0 }, CancellationToken.None);

        capturedQuery.Should().NotBeNull();
        capturedQuery!.Page.Should().Be(1);
        capturedQuery.PageSize.Should().Be(30);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value.Should().BeAssignableTo<SimilarPaintListResponse>().Subject;
        response.Items.Should().ContainSingle(x => x.Id == "p2" && x.Similarity == 2.1);
        response.Page.Should().Be(1);
        response.PageSize.Should().Be(30);
    }

    [Fact]
    public async Task GetById_ShouldReturnMappedPaintResponse()
    {
        var sender = new Mock<ISender>();
        sender
            .Setup(x => x.Send(It.IsAny<GetPaintByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPaintByIdResult
            {
                Id = "p1",
                Name = "Red",
                ModelNumber = "C1",
                BrandName = "Mr.Color",
                PaintTypeName = "Lacquer",
                GlossName = "Gloss",
                Price = 220,
                Hex = "#ff0000",
                Rgb_R = 255,
                Rgb_G = 0,
                Rgb_B = 0,
                Hsl_H = 0,
                Hsl_S = 100,
                Hsl_L = 50,
                Tags = [new GetTagByIdResult { Id = "t1", Name = "Warm", CategoryId = "cat1" }],
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

        var controller = new PaintsController(sender.Object);

        var result = await controller.GetById("p1", CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value.Should().BeAssignableTo<PaintResponse>().Subject;
        response.Id.Should().Be("p1");
        response.Brand.Should().Be("Mr.Color");
        response.Tags.Should().ContainSingle(x => x.Id == "t1" && x.Category == "cat1");
    }
}
