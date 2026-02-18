using PlasticModelApp.Domain.Catalog.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.Services;


/// <summary>
/// Color Difference Calculator
/// Utility class for calculating color differences between RGB colors using the CIEDE2000 formula.
/// </summary>
public static class ColorDifferenceCalculator
{
    /// <summary>
    /// Calculates the CIEDE2000 color difference between two RGB colors.
    /// </summary>
    /// <param name="first">The first RGB color.</param>
    /// <param name="second">The second RGB color.</param>
    /// <returns>The CIEDE2000 color difference.</returns>
    public static double CalculateCiede2000(RgbColor first, RgbColor second)
        => CalculateCiede2000(first.R, first.G, first.B, second.R, second.G, second.B);

    /// <summary>
    /// Calculates the CIEDE2000 color difference between two RGB colors.
    /// </summary>
    /// <param name="r1">The red component of the first color (0-255).</param>
    /// <param name="g1">The green component of the first color (0-255).</param>
    /// <param name="b1">The blue component of the first color (0-255).</param>
    /// <param name="r2">The red component of the second color (0-255).</param>
    /// <param name="g2">The green component of the second color (0-255).</param>
    /// <param name="b2">The blue component of the second color (0-255).</param>
    /// <returns>The CIEDE2000 color difference.</returns>
    public static double CalculateCiede2000(int r1, int g1, int b1, int r2, int g2, int b2)
    {
        var lab1 = ToLab(r1, g1, b1);
        var lab2 = ToLab(r2, g2, b2);
        return CalculateCiede2000(lab1, lab2);
    }

    /// <summary>
    /// Converts RGB color to Lab color.
    /// </summary>
    /// <param name="r">Red component (0-255)</param>
    /// <param name="g">Green component (0-255)</param>
    /// <param name="b">Blue component (0-255)</param>
    /// <returns>Lab color (L, A, B)</returns>
    private static (double L, double A, double B) ToLab(int r, int g, int b)
    {
        double rn = PivotRgb(r / 255d);
        double gn = PivotRgb(g / 255d);
        double bn = PivotRgb(b / 255d);

        // sRGB (D65) -> XYZ
        double x = rn * 0.4124564 + gn * 0.3575761 + bn * 0.1804375;
        double y = rn * 0.2126729 + gn * 0.7151522 + bn * 0.0721750;
        double z = rn * 0.0193339 + gn * 0.1191920 + bn * 0.9503041;

        // Normalize for D65 white point
        double fx = PivotLab(x / 0.95047);
        double fy = PivotLab(y / 1.00000);
        double fz = PivotLab(z / 1.08883);

        double L = Math.Max(0, 116 * fy - 16);
        double A = 500 * (fx - fy);
        double B = 200 * (fy - fz);
        return (L, A, B);
    }

    /// <summary>
    /// Converts RGB component to linear RGB.
    /// </summary>
    /// <param name="value">The RGB component value (0-255)</param>
    /// <returns>The linear RGB value (0-1)</returns>
    private static double PivotRgb(double value)
        => value > 0.04045 ? Math.Pow((value + 0.055) / 1.055, 2.4) : value / 12.92;

    /// <summary>
    /// Converts Lab color to RGB color.
    /// </summary>
    /// <param name="L">Lightness (0-100)</param>
    /// <param name="A">Green-Red component (-128 to 127)</param>
    /// <param name="B">Blue-Yellow component (-128 to 127)</param>
    /// <returns>Converted RGB color (R, G, B)</returns>
    private static double PivotLab(double value)
        => value > 0.008856 ? Math.Pow(value, 1.0 / 3.0) : (903.3 * value + 16) / 116;

    /// <summary>
    /// Calculates the CIEDE2000 color difference between two Lab colors.
    /// </summary>
    /// <param name="lab1">The first Lab color</param>
    /// <param name="lab2">The second Lab color</param>
    /// <returns>The CIEDE2000 color difference</returns>
    private static double CalculateCiede2000((double L, double A, double B) lab1, (double L, double A, double B) lab2)
    {
        const double Deg2Rad = Math.PI / 180.0;
        const double Rad2Deg = 180.0 / Math.PI;

        double avgLp = (lab1.L + lab2.L) / 2.0;
        double c1 = Math.Sqrt(lab1.A * lab1.A + lab1.B * lab1.B);
        double c2 = Math.Sqrt(lab2.A * lab2.A + lab2.B * lab2.B);
        double avgC = (c1 + c2) / 2.0;

        double g = 0.5 * (1 - Math.Sqrt(Math.Pow(avgC, 7) / (Math.Pow(avgC, 7) + Math.Pow(25.0, 7))));
        double a1p = (1 + g) * lab1.A;
        double a2p = (1 + g) * lab2.A;
        double c1p = Math.Sqrt(a1p * a1p + lab1.B * lab1.B);
        double c2p = Math.Sqrt(a2p * a2p + lab2.B * lab2.B);
        double avgCp = (c1p + c2p) / 2.0;

        double h1p = Math.Atan2(lab1.B, a1p);
        if (h1p < 0) h1p += 2 * Math.PI;
        double h2p = Math.Atan2(lab2.B, a2p);
        if (h2p < 0) h2p += 2 * Math.PI;

        double deltahp;
        if (c1p * c2p == 0)
        {
            deltahp = 0;
        }
        else if (Math.Abs(h1p - h2p) <= Math.PI)
        {
            deltahp = h2p - h1p;
        }
        else if (h2p <= h1p)
        {
            deltahp = h2p - h1p + 2 * Math.PI;
        }
        else
        {
            deltahp = h2p - h1p - 2 * Math.PI;
        }

        double deltaLp = lab2.L - lab1.L;
        double deltaCp = c2p - c1p;
        double deltaHp = 2 * Math.Sqrt(c1p * c2p) * Math.Sin(deltahp / 2.0);

        double avgHp;
        if (c1p * c2p == 0)
        {
            avgHp = h1p + h2p;
        }
        else if (Math.Abs(h1p - h2p) <= Math.PI)
        {
            avgHp = (h1p + h2p) / 2.0;
        }
        else if (h1p + h2p < 2 * Math.PI)
        {
            avgHp = (h1p + h2p + 2 * Math.PI) / 2.0;
        }
        else
        {
            avgHp = (h1p + h2p - 2 * Math.PI) / 2.0;
        }

        double t = 1
            - 0.17 * Math.Cos(avgHp - Deg2Rad * 30)
            + 0.24 * Math.Cos(2 * avgHp)
            + 0.32 * Math.Cos(3 * avgHp + Deg2Rad * 6)
            - 0.20 * Math.Cos(4 * avgHp - Deg2Rad * 63);

        double deltaTheta = Deg2Rad * 30 * Math.Exp(-Math.Pow((avgHp * Rad2Deg - 275) / 25, 2));
        double rc = 2 * Math.Sqrt(Math.Pow(avgCp, 7) / (Math.Pow(avgCp, 7) + Math.Pow(25.0, 7)));
        double sl = 1 + ((0.015 * Math.Pow(avgLp - 50, 2)) / Math.Sqrt(20 + Math.Pow(avgLp - 50, 2)));
        double sc = 1 + 0.045 * avgCp;
        double sh = 1 + 0.015 * avgCp * t;
        double rt = -Math.Sin(2 * deltaTheta) * rc;

        double deltaE = Math.Sqrt(
            Math.Pow(deltaLp / sl, 2) +
            Math.Pow(deltaCp / sc, 2) +
            Math.Pow(deltaHp / sh, 2) +
            rt * (deltaCp / sc) * (deltaHp / sh));

        return Math.Round(deltaE, 4);
    }
}
