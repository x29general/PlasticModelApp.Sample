using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class TagCategoryTests
{
    [Fact]
    public void Create_And_Update_Should_Work()
    {
        var tc = TagCategory.Create("TC1", "Cat1");
        tc.Id.Value.Should().Be("TC1");
        tc.Name.Should().Be("Cat1");

        tc.Update("Cat2", "desc");
        tc.Name.Should().Be("Cat2");
        tc.Description.Should().Be("desc");
    }

    [Fact]
    public void Update_Should_Throw_When_Name_Empty()
    {
        var tc = TagCategory.Create("TC1", "Cat1");
        Action act = () => tc.Update(" ", null);
        act.Should().Throw<ArgumentException>();
    }
}

