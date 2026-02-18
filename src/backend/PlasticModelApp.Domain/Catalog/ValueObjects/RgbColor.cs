using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// RGB Color Value Object
/// </summary>
public sealed class RgbColor : ValueObject
{
    public int R { get; private set; }
    public int G { get; private set; }
    public int B { get; private set; }

    /// <summary>
    /// Initialize a new instance of RgbColor.
    /// </summary>
    /// <param name="r">Red (0-255)</param>
    /// <param name="g">Green (0-255)</param>
    /// <param name="b">Blue (0-255)</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any component is outside the range of 0-255.</exception>
    public RgbColor(int r, int g, int b)
    {
        if (r is < 0 or > 255) throw new ArgumentOutOfRangeException(nameof(r), "R must be between 0 and 255.");
        if (g is < 0 or > 255) throw new ArgumentOutOfRangeException(nameof(g), "G must be between 0 and 255.");
        if (b is < 0 or > 255) throw new ArgumentOutOfRangeException(nameof(b), "B must be between 0 and 255.");
 
        R = r;
        G = g;
        B = b;
    }

    private RgbColor() { }
 
    /// <summary>
    /// Returns the components that define the equality of the value object.
    /// </summary>
    /// <returns>The components that define the equality of the value object.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return R;
        yield return G;
        yield return B;
    }
}
