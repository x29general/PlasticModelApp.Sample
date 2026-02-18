using System.Text.RegularExpressions;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;
 
 /// <summary>
 /// Hex Color Value Object
 /// </summary>
 public sealed class HexColor : ValueObject
 {
    /// <summary>
    /// Value of the Hex Color in "#RRGGBB" format
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Regular expression pattern used for validating HEX color codes.
    /// </summary>
    private static readonly Regex HexColorRegex = new("^#([A-Fa-f0-9]{6})$");

    /// <summary>
    /// Initializes a new instance of the HexColor class.
    /// </summary>
    /// <param name="value">The HEX color code in "#RRGGBB" format.</param>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or invalid format.</exception>
     public HexColor(string value)
     {
         if (string.IsNullOrWhiteSpace(value) || !HexColorRegex.IsMatch(value))
             throw new ArgumentException("HexColor must be in the format '#RRGGBB'.", nameof(value));
 
         Value = value;
     }
 
     /// <summary>
     /// Returns the components that define the equality of the value object.
     /// </summary>
     protected override IEnumerable<object?> GetEqualityComponents()
     {
         yield return Value;
     }
 
     private HexColor() { }
 }
 
