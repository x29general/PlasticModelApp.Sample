using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class RgbColorTests
{
    [Fact]
    public void Ctor_Should_Accept_Boundaries_0_255()
    {
        var min = new RgbColor(0, 0, 0);
        min.R.Should().Be(0);
        var max = new RgbColor(255, 255, 255);
        max.B.Should().Be(255);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 0, -1)]
    [InlineData(256, 0, 0)]
    [InlineData(0, 256, 0)]
    [InlineData(0, 0, 256)]
    public void Ctor_Should_Throw_When_OutOfRange(int r, int g, int b)
    {
        Action act = () => _ = new RgbColor(r, g, b);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}

