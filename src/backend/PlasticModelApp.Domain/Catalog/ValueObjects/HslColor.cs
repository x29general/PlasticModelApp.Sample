using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// Hsl Color Value Object
/// </summary>
public sealed class HslColor : ValueObject
{
    public float H { get; private set; }
    public float S { get; private set; }
    public float L { get; private set; }

    /// <summary>
    /// Creates a new instance of HslColor.
    /// </summary>
    /// <param name="h">Hue (0-360)</param>
    /// <param name="s">Saturation (0-100)</param>
    /// <param name="l">Lightness (0-100)</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when values are out of range.</exception>
    public HslColor(float h, float s, float l)
    {
        if (h is < 0 or > 360) throw new ArgumentOutOfRangeException(nameof(h), "H must be between 0 and 360.");
        if (s is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(s), "S must be between 0 and 100.");
        if (l is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(l), "L must be between 0 and 100.");

        H = h;
        S = s;
        L = l;
    }

    private HslColor() { }

    /// <summary>
    /// Returns the components that define the equality of the value object.
    /// </summary>
    /// <returns>The components used for equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        // Round to 4 decimal places for equality comparison
        yield return MathF.Round(H, 4);
        yield return MathF.Round(S, 4);
        yield return MathF.Round(L, 4);
    }
}
