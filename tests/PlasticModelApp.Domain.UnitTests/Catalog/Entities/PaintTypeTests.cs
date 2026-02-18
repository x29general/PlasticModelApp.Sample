using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class PaintTypeTests
{
    [Fact]
    public void Create_And_Update_Should_Work()
    {
        var pt = PaintType.Create("T1", "Type1");
        pt.Id.Value.Should().Be("T1");
        pt.Name.Should().Be("Type1");

        pt.Update("Type2", "desc");
        pt.Name.Should().Be("Type2");
        pt.Description.Should().Be("desc");
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Empty()
    {
        Action act = () => _ = PaintType.Create("T1", " ");
        act.Should().Throw<ArgumentException>();
    }
}

