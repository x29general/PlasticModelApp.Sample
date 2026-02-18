using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class TagTests
{
    private static readonly DateTimeOffset Now = new(2020, 1, 2, 3, 4, 5, TimeSpan.Zero);
    private static readonly DateTimeOffset Later = new(2020, 1, 2, 4, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Create_And_Update_Should_Work()
    {
        var tc = TagCategoryId.From("TC1");
        var tag = Tag.Create("T1", "TagA", tc, Now, hex: "#112233");
        tag.Name.Should().Be("TagA");
        tag.Hex.Should().Be("#112233");

        tag.Update("TagB", tc, Later, hex: "#AABBCC");
        tag.Name.Should().Be("TagB");
        tag.Hex.Should().Be("#AABBCC");
        tag.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Update_Should_Throw_When_InvalidHex()
    {
        var tc = TagCategoryId.From("TC1");
        var tag = Tag.Create("T1", "TagA", tc, Now);
        Action act = () => tag.Update("TagA", tc, Later, hex: "invalid");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_Should_Throw_When_InvalidHex()
    {
        var tc = TagCategoryId.From("TC1");
        Action act = () => _ = Tag.Create("T1", "TagA", tc, Now, hex: "invalid");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void MarkAsDeleted_Should_Toggle_And_Prevent_Double_Delete()
    {
        var tc = TagCategoryId.From("TC1");
        var tag = Tag.Create("T1", "TagA", tc, Now);
        tag.MarkAsDeleted(Later);
        tag.IsDeleted.Should().BeTrue();
        Action act = () => tag.MarkAsDeleted(Later);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Update_Should_Throw_When_Deleted()
    {
        var tc = TagCategoryId.From("TC1");
        var tag = Tag.Create("T1", "TagA", tc, Now);
        tag.MarkAsDeleted(Later);
        Action act = () => tag.Update("X", tc, Later);
        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData("#000000")]
    [InlineData("#ffffff")]
    [InlineData("#AaBbCc")]
    public void Create_Should_Accept_Valid_Hex_Boundaries(string hex)
    {
        var tc = TagCategoryId.From("TC1");
        var tag = Tag.Create("T1", "TagA", tc, Now, hex: hex);

        tag.Hex.Should().Be(hex);
    }
}
