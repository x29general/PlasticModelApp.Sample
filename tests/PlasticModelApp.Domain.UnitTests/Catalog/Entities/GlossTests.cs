using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class GlossTests
{
    [Fact]
    public void Create_And_Update_Should_Work()
    {
        var g = Gloss.Create("G1", "Gloss1");
        g.Id.Value.Should().Be("G1");
        g.Name.Should().Be("Gloss1");

        g.Update("Gloss2", "desc");
        g.Name.Should().Be("Gloss2");
        g.Description.Should().Be("desc");
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Empty()
    {
        Action act = () => _ = Gloss.Create("G1", " ");
        act.Should().Throw<ArgumentException>();
    }
}

