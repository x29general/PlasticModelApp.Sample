using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class PriceTests
{
    [Fact]
    public void Ctor_Should_Round_To_2dp_AwayFromZero()
    {
        new Price(123.234m).Amount.Should().Be(123.23m);
        new Price(123.235m).Amount.Should().Be(123.24m);
        new Price(0m).Amount.Should().Be(0m);
    }

    [Fact]
    public void Ctor_Should_Throw_When_Negative()
    {
        var act = () => new Price(-0.01m);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Equality_Should_Work_By_Value()
    {
        var a = new Price(10m);
        var b = new Price(10.000m);
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}

