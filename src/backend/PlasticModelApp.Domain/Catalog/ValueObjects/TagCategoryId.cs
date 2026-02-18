using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// Tag Category Identifier Value Object
/// </summary>
public sealed class TagCategoryId : ValueObject
{
    /// <summary>
    /// The value of the identifier
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Creates a TagCategoryId from a string.
    /// </summary>
    /// <param name="value">The value of the identifier</param>
    /// <exception cref="ArgumentException">Thrown when value is null/empty/whitespace</exception>
    public TagCategoryId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("TagCategoryId cannot be empty.", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Creates a TagCategoryId from an existing identifier string.
    /// </summary>
    public static TagCategoryId From(string value) => new(value);

    /// <summary>
    /// Returns the string representation of the TagCategoryId.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Returns the components that define the equality of the value object.
    /// </summary>
    /// <returns>The components that define the equality of the value object.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
