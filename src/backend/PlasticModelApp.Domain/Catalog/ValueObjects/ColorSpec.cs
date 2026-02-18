using System.Text.RegularExpressions;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// Color specification value object containing HEX, RGB, and HSL representations.
/// </summary>
public sealed class ColorSpec : ValueObject
{
    /// <summary>
    /// HEX
    /// </summary>
    public HexColor Hex { get; private init; } = null!;
 
    /// <summary>
    /// RGB
    /// </summary>
    public RgbColor Rgb { get; private init; } = null!;
 
    /// <summary>
    /// HSL
    /// </summary>
    public HslColor Hsl { get; private init; } = null!;
 
    /// <summary>
    /// Regular expression for validating HEX color codes in the format '#RRGGBB'.
    /// </summary>
    private static readonly Regex HexRegex = new("^#([A-Fa-f0-9]{6})$");
 
    /// <summary>
    /// ColorSpec constructor for creating a color specification.
    /// </summary>
    /// <param name="hex">'#RRGGBB' format HEX color code.</param>
    /// <param name="rgb">RGB color representation.</param>
    /// <param name="hsl">HSL color representation.</param>
    /// <exception cref="ArgumentException">hex is null, empty, or whitespace.</exception>
    /// <returns>A new ColorSpec instance.</returns>
    public ColorSpec(string hex, RgbColor? rgb = null, HslColor? hsl = null)
    {
        if (string.IsNullOrWhiteSpace(hex))
            throw new ArgumentException("Hex code cannot be empty.", nameof(hex));
        if (!HexRegex.IsMatch(hex))
            throw new ArgumentException("Hex code must be in the format '#RRGGBB'.", nameof(hex));

        Hex = new HexColor(hex);
        var derivedRgb = HexToRgb(hex);
        var derivedHsl = HexToHsl(hex);

        // Hex is the single source of truth.
        if (rgb is not null && !rgb.Equals(derivedRgb))
            throw new ArgumentException("RGB must match the value derived from hex.", nameof(rgb));
        if (hsl is not null && !hsl.Equals(derivedHsl))
            throw new ArgumentException("HSL must match the value derived from hex.", nameof(hsl));

        Rgb = derivedRgb;
        Hsl = derivedHsl;
    }
 
    private ColorSpec() { }
 
    /// <summary>
    /// Returns the components that define the equality of the value object.
    /// </summary>
    /// <returns>The components used for equality comparison.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Hex;
        yield return Rgb;
        yield return MathF.Round(Hsl.H, 4);
        yield return MathF.Round(Hsl.S, 4);
        yield return MathF.Round(Hsl.L, 4);
    }

    /// <summary>
    /// Converts a HEX color code to its RGB representation.
    /// </summary>
    /// <param name="hex">'#RRGGBB' format HEX color code.</param>
    /// <returns>The RGB representation of the color.</returns>
    public static RgbColor HexToRgb(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex) || !HexRegex.IsMatch(hex))
            throw new ArgumentException("Hex code must be in the format '#RRGGBB'.", nameof(hex));
 
        int r = Convert.ToInt32(hex.Substring(1, 2), 16);
        int g = Convert.ToInt32(hex.Substring(3, 2), 16);
        int b = Convert.ToInt32(hex.Substring(5, 2), 16);
 
        return new RgbColor(r, g, b);
    }

    /// <summary>
    /// Converts a HEX color code to its HSL representation.
    /// </summary>
    /// <param name="hex">'#RRGGBB' format HEX color code.</param>
    /// <returns>The HSL representation of the color.</returns>
    public static HslColor HexToHsl(string hex)
    {
        var rgb = HexToRgb(hex);
        float r = rgb.R / 255f;
        float g = rgb.G / 255f;
        float b = rgb.B / 255f;
 
        float max = Math.Max(r, Math.Max(g, b));
        float min = Math.Min(r, Math.Min(g, b));
        float h = 0, s, l = (max + min) / 2;
 
        if (max == min)
        {
            h = s = 0; // achromatic
        }
        else
        {
            float d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
 
            if (max == r)
                h = (g - b) / d + (g < b ? 6 : 0);
            else if (max == g)
                h = (b - r) / d + 2;
            else if (max == b)
                h = (r - g) / d + 4;
 
            h /= 6;
        }
 
        return new HslColor(h * 360, s * 100, l * 100);
    }
}

