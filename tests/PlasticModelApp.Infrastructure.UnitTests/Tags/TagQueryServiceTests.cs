using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Infrastructure.Queries;
using PlasticModelApp.Infrastructure.UnitTests.Fixtures;

namespace PlasticModelApp.Infrastructure.UnitTests.Tags;

public sealed class TagQueryServiceTests
{
    [Fact]
    public async Task GetByIdAsync_Should_Return_Tag()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new TagQueryService(context);

        var result = await sut.GetByIdAsync(new TagId(TestDataSeeder.Ids.TagMatte));

        result.Should().NotBeNull();
        result!.Name.Should().Be("Matte");
        result.CategoryId.Should().Be(TestDataSeeder.Ids.CategoryFinish);
    }

    [Fact]
    public async Task GetByCategoryAsync_Should_Filter_By_CategoryName_And_Exclude_SoftDeleted()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new TagQueryService(context);

        var result = await sut.GetByCategoryAsync("Tone");

        result.Select(x => x.Id).Should().Equal(TestDataSeeder.Ids.TagWarm);
    }
}
