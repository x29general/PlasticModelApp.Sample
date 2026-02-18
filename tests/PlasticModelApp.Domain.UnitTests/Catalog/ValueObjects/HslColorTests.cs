using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class HslColorTests
{
    [Fact]
    public void Ctor_Should_Accept_Boundaries()
    {
        var min = new HslColor(0, 0, 0);
        min.H.Should().Be(0);
        var max = new HslColor(360, 100, 100);
        max.L.Should().Be(100);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(361, 0, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 101, 0)]
    [InlineData(0, 0, -1)]
    [InlineData(0, 0, 101)]
    public void Ctor_Should_Throw_When_OutOfRange(float h, float s, float l)
    {
        Action act = () => _ = new HslColor(h, s, l);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}

