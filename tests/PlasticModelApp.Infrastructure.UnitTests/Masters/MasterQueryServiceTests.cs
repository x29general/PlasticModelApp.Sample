using FluentAssertions;
using PlasticModelApp.Infrastructure.Queries;
using PlasticModelApp.Infrastructure.UnitTests.Fixtures;

namespace PlasticModelApp.Infrastructure.UnitTests.Masters;

public sealed class MasterQueryServiceTests
{
    [Fact]
    public async Task GetAllAsync_Should_Return_All_MasterData_Ordered_By_Name()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new MasterQueryService(context);

        var result = await sut.GetAllAsync();

        result.Brands.Select(x => x.Name).Should().Equal("Brand A", "Brand B");
        result.PaintTypes.Select(x => x.Name).Should().Equal("Acrylic", "Enamel");
        result.Glosses.Select(x => x.Name).Should().Equal("Gloss", "Matte");
        result.TagCategories.Select(x => x.Name).Should().Equal("Finish", "Tone");
        result.Tags.Should().Contain(x => x.CategoryId == TestDataSeeder.Ids.CategoryFinish);
    }
}
