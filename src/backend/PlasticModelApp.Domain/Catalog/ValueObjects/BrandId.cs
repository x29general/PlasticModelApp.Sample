using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// Identifier for a brand in the catalog.
/// </summary>
public sealed class BrandId : ValueObject
{
     /// <summary>
     /// The identifier value.
     /// </summary>
     public string Value { get; private set; } = string.Empty;
 
     /// <summary>
     /// Creates a BrandId from the given string.
     /// </summary>
     /// <param name="value">The identifier value.</param>
     /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace.</exception>
     public BrandId(string value)
     {
         if (string.IsNullOrWhiteSpace(value))
             throw new ArgumentException("BrandId cannot be empty.", nameof(value));
 
         Value = value;
     }
     
     /// <summary>
     /// Creates a BrandId from an existing identifier string.
     /// </summary>
     public static BrandId From(string value) => new(value);
 
     /// <summary>
     /// Returns the string representation of the identifier.
     /// </summary>
     /// <returns>The string representation of the identifier.</returns>
     public override string ToString() => Value;
 
     /// <summary>
     /// Returns the components that define the equality of the value object.
     /// </summary>
     /// <returns>The components used for equality comparison.</returns>
     protected override IEnumerable<object?> GetEqualityComponents()
     {
         yield return Value;
     }
 }
