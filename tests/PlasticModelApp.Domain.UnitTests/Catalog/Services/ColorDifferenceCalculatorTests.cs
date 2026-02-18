using FluentAssertions;
using PlasticModelApp.Domain.Catalog.Services;
using PlasticModelApp.Domain.Catalog.ValueObjects;
using Xunit;

namespace PlasticModelApp.Domain.UnitTests.Catalog.Services;

public class ColorDifferenceCalculatorTests
{
    [Fact]
    public void CalculateCiede2000_Should_Return_Zero_For_Same_Color()
    {
        var d = ColorDifferenceCalculator.CalculateCiede2000(17, 34, 51, 17, 34, 51);
        d.Should().Be(0);
    }

    [Fact]
    public void CalculateCiede2000_Should_Be_Symmetric()
    {
        var a = new RgbColor(255, 0, 0);
        var b = new RgbColor(0, 255, 0);

        var d1 = ColorDifferenceCalculator.CalculateCiede2000(a, b);
        var d2 = ColorDifferenceCalculator.CalculateCiede2000(b, a);

        d1.Should().Be(d2);
    }

    [Fact]
    public void CalculateCiede2000_Should_Be_Larger_For_Distant_Colors()
    {
        var near = ColorDifferenceCalculator.CalculateCiede2000(255, 0, 0, 254, 0, 0);
        var far = ColorDifferenceCalculator.CalculateCiede2000(255, 0, 0, 0, 255, 0);

        far.Should().BeGreaterThan(near);
    }
}
