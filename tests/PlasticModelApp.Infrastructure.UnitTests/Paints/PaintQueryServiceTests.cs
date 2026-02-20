using FluentAssertions;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using PlasticModelApp.Infrastructure.Queries;
using PlasticModelApp.Infrastructure.UnitTests.Fixtures;

namespace PlasticModelApp.Infrastructure.UnitTests.Paints;

public sealed class PaintQueryServiceTests
{
    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_NotFound()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var result = await sut.GetByIdAsync(new PaintId("not-found"));

        result.Should().BeNull();
    }

    [Fact]
    public async Task SearchAsync_Should_Apply_Tag_Group_Logic_And_Filter_SoftDeleted()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var criteria = new Criteria
        {
            TagIds = [TestDataSeeder.Ids.TagMatte, TestDataSeeder.Ids.TagWarm],
            Sort = SearchPaintsSortOption.NameAsc,
            Page = 1,
            PageSize = 10
        };

        var result = await sut.SearchAsync(new SearchPaintsQuery(criteria));

        result.Total.Should().Be(1);
        result.Items.Select(x => x.Id).Should().Equal(TestDataSeeder.Ids.PaintP1);
    }

    [Fact]
    public async Task SearchAsync_Should_Sort_By_ModelNumberAsc()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var criteria = new Criteria
        {
            Sort = SearchPaintsSortOption.ModelNumberAsc,
            Page = 1,
            PageSize = 10
        };

        var result = await sut.SearchAsync(new SearchPaintsQuery(criteria));

        result.Items.Select(x => x.Id).Should().Equal(
            TestDataSeeder.Ids.PaintP1,
            TestDataSeeder.Ids.PaintP2,
            TestDataSeeder.Ids.PaintP3);
    }

    [Fact]
    public async Task SearchAsync_Should_Sort_By_NameDesc()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var criteria = new Criteria
        {
            Sort = SearchPaintsSortOption.NameDesc,
            Page = 1,
            PageSize = 10
        };

        var result = await sut.SearchAsync(new SearchPaintsQuery(criteria));

        result.Items.Select(x => x.Name).Should().Equal("Charlie Blue", "Bravo Red", "Alpha Red");
    }

    [Fact]
    public async Task SearchAsync_Should_Sort_By_ModelNumberDesc()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var criteria = new Criteria
        {
            Sort = SearchPaintsSortOption.ModelNumberDesc,
            Page = 1,
            PageSize = 10
        };

        var result = await sut.SearchAsync(new SearchPaintsQuery(criteria));

        result.Items.Select(x => x.Id).Should().Equal(
            TestDataSeeder.Ids.PaintP3,
            TestDataSeeder.Ids.PaintP2,
            TestDataSeeder.Ids.PaintP1);
    }

    [Fact]
    public async Task SearchAsync_Should_Return_Empty_When_NoMatch()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var criteria = new Criteria
        {
            BrandIds = ["no-brand"],
            Sort = SearchPaintsSortOption.NameAsc,
            Page = 1,
            PageSize = 10
        };

        var result = await sut.SearchAsync(new SearchPaintsQuery(criteria));

        result.Total.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_Should_Project_MasterNames_And_Tags()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var result = await sut.GetByIdAsync(new PaintId(TestDataSeeder.Ids.PaintP1));

        result.Should().NotBeNull();
        result!.BrandName.Should().Be("Brand A");
        result.PaintTypeName.Should().Be("Acrylic");
        result.GlossName.Should().Be("Matte");
        result.Tags.Select(t => t.Id).Should().Contain([TestDataSeeder.Ids.TagMatte, TestDataSeeder.Ids.TagWarm]);
    }

    [Fact]
    public async Task SearchSimilarAsync_Should_Return_Empty_When_NoPaints()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var query = new SearchSimilarPaintsQuery(r: 255, g: 0, b: 0, threshold: 10d, page: 1, pageSize: 1);

        var result = await sut.SearchSimilarAsync(query);

        result.Total.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchSimilarAsync_Should_Apply_Threshold_And_Paging()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var query = new SearchSimilarPaintsQuery(r: 255, g: 0, b: 0, threshold: 10d, page: 1, pageSize: 1);

        var result = await sut.SearchSimilarAsync(query);

        result.Total.Should().BeGreaterThanOrEqualTo(1);
        result.Items.Should().HaveCount(1);
        result.Items[0].Id.Should().Be(TestDataSeeder.Ids.PaintP1);
    }

    [Fact]
    public async Task ListByIdsAsync_Should_Return_Empty_For_Empty_Input()
    {
        using var factory = new SqliteDbContextFactory();
        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var result = await sut.ListByIdsAsync([]);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ListByIdsAsync_Should_Return_Items_In_Input_Order()
    {
        using var factory = new SqliteDbContextFactory();
        using (var seedContext = factory.CreateContext())
        {
            TestDataSeeder.SeedCommon(seedContext);
        }

        using var context = factory.CreateContext();
        var sut = new PaintQueryService(context);

        var ids = new[]
        {
            new PaintId(TestDataSeeder.Ids.PaintP3),
            new PaintId(TestDataSeeder.Ids.PaintP1),
        };

        var result = await sut.ListByIdsAsync(ids);

        result.Select(x => x.Id).Should().Equal(TestDataSeeder.Ids.PaintP3, TestDataSeeder.Ids.PaintP1);
    }
}
