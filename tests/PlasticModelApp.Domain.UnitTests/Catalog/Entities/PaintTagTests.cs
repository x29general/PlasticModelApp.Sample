using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Entities;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.UnitTests.Entities;

public class PaintTagTests
{
    [Fact]
    public void Create_Should_Set_Ids()
    {
        var pt = PaintTag.Create(new PaintId("P1"), TagId.From("T1"));
        pt.PaintId.Value.Should().Be("P1");
        pt.TagId.Value.Should().Be("T1");
    }

    [Fact]
    public void Create_Should_Throw_When_PaintId_Is_Null()
    {
        Action act = () => _ = PaintTag.Create(null!, TagId.From("T1"));
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_Should_Throw_When_TagId_Is_Null()
    {
        Action act = () => _ = PaintTag.Create(new PaintId("P1"), null!);
        act.Should().Throw<ArgumentNullException>();
    }
}

