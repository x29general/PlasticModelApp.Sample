using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class BrandTests
{
    [Fact]
    public void Create_Should_Set_Properties()
    {
        var b = Brand.Create("B1", "Brand1", "desc");
        b.Id.Value.Should().Be("B1");
        b.Name.Should().Be("Brand1");
        b.Description.Should().Be("desc");
    }

    [Fact]
    public void Update_Should_Change_Name_And_Description()
    {
        var b = Brand.Create("B1", "Brand1");
        b.Update("B2", "x");
        b.Name.Should().Be("B2");
        b.Description.Should().Be("x");
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Empty()
    {
        Action act = () => _ = Brand.Create("B1", " ");
        act.Should().Throw<ArgumentException>();
    }
}

