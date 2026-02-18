using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class ModelNumberTests
{
    [Fact]
    public void Ctor_Should_Parse_Prefix_And_Number()
    {
        var m1 = new ModelNumber("BR-123");
        m1.Value.Should().Be("BR-123");
        m1.SortPrefix.Should().Be("BR-");
        m1.SortNumber.Should().Be(123);

        var m2 = new ModelNumber("123");
        m2.SortPrefix.Should().BeNull();
        m2.SortNumber.Should().Be(123);

        var m3 = new ModelNumber("BR");
        m3.SortPrefix.Should().Be("BR");
        m3.SortNumber.Should().BeNull();
    }

    [Fact]
    public void Ctor_Should_Throw_When_Empty_Or_TooLong()
    {
        Action empty = () => _ = new ModelNumber("");
        empty.Should().Throw<ArgumentException>();

        var longStr = new string('A', 51);
        Action tooLong = () => _ = new ModelNumber(longStr);
        tooLong.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Ctor_Should_Set_SortNumber_Null_When_Number_Overflows_Int()
    {
        var m = new ModelNumber("BR-21474836480");

        m.SortPrefix.Should().Be("BR-");
        m.SortNumber.Should().BeNull();
    }
}

