using System;
using FluentAssertions;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.ValueObjects;

public class ColorSpecTests
{
    [Fact]
    public void Ctor_Should_Set_Hex_And_Derived_Colors()
    {
        var color = new ColorSpec("#AABBCC");
        color.Hex.Value.Should().Be("#AABBCC");
        color.Rgb.R.Should().Be(170);
        color.Rgb.G.Should().Be(187);
        color.Rgb.B.Should().Be(204);

        // HSL は計算誤差を考慮して範囲で検証
        color.Hsl.H.Should().BeGreaterThan(200f).And.BeLessThan(230f);
        color.Hsl.S.Should().BeGreaterThan(20f).And.BeLessThan(40f);
        color.Hsl.L.Should().BeGreaterThan(70f).And.BeLessThan(85f);
    }

    [Fact]
    public void HexToRgb_Should_Convert_Correctly()
    {
        var rgb = ColorSpec.HexToRgb("#112233");
        rgb.R.Should().Be(17);
        rgb.G.Should().Be(34);
        rgb.B.Should().Be(51);
    }

    [Fact]
    public void Ctor_Should_Throw_When_Invalid_Hex()
    {
        Action act = () => _ = new ColorSpec("112233");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Ctor_Should_Accept_Matching_Rgb_And_Hsl()
    {
        var rgb = ColorSpec.HexToRgb("#112233");
        var hsl = ColorSpec.HexToHsl("#112233");

        var color = new ColorSpec("#112233", rgb, hsl);

        color.Rgb.Should().Be(rgb);
        color.Hsl.Should().Be(hsl);
    }

    [Fact]
    public void Ctor_Should_Throw_When_Rgb_Does_Not_Match_Hex()
    {
        Action act = () => _ = new ColorSpec("#112233", new RgbColor(255, 255, 255), null);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Ctor_Should_Throw_When_Hsl_Does_Not_Match_Hex()
    {
        Action act = () => _ = new ColorSpec("#112233", null, new HslColor(0, 0, 0));
        act.Should().Throw<ArgumentException>();
    }
}

