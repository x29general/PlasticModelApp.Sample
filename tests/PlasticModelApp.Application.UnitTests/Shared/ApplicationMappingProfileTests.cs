using AutoMapper;
using FluentAssertions;
using PlasticModelApp.Application.Masters.Queries;
using PlasticModelApp.Application.Shared;
using PlasticModelApp.Domain.Catalog.Entities;
using Xunit;

namespace PlasticModelApp.Application.UnitTests.Shared;

public class ApplicationMappingProfileTests
{
    private readonly IMapper _mapper;

    public ApplicationMappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Constructor_Can_Be_Initialized()
    {
        var profile = new ApplicationMappingProfile();
        profile.Should().NotBeNull();
    }

    [Fact]
    public void Map_Master_Entities_To_Master_Results()
    {
        var brand = Brand.Create("B1", "Brand");
        var gloss = Gloss.Create("G1", "Gloss");
        var paintType = PaintType.Create("PT1", "Type");
        var tagCategory = TagCategory.Create("TC1", "Category");

        var brandResult = _mapper.Map<GetBrandResult>(brand);
        var glossResult = _mapper.Map<GetGlossResult>(gloss);
        var paintTypeResult = _mapper.Map<GetPaintTypeResult>(paintType);
        var tagCategoryResult = _mapper.Map<GetTagCategoryResult>(tagCategory);

        brandResult.Id.Should().Be("B1");
        glossResult.Id.Should().Be("G1");
        paintTypeResult.Id.Should().Be("PT1");
        tagCategoryResult.Id.Should().Be("TC1");
    }
}
