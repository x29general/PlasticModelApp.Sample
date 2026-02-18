using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// Model Number Value Object
/// </summary>
public sealed class ModelNumber : ValueObject
{
    /// <summary>
    /// Value of the Model Number
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Prefix part for sorting the Model Number
    /// </summary>
    public string? SortPrefix { get; private set; }

    /// <summary>
    /// Numeric part for sorting the Model Number
    /// </summary>
    public int? SortNumber { get; private set; }

    /// <summary>
    /// Implicit conversion to string
    /// </summary>
    public static implicit operator string(ModelNumber m) => m.Value;

    /// <summary>
    /// Returns the string representation of the identifier
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Returns the components that define the equality of the value object.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Creates a ModelNumber from the specified string.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace.</exception>
    public ModelNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ModelNumber cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("ModelNumber must be 50 characters or fewer.", nameof(value));

        Value = value;
        (SortPrefix, SortNumber) = ParseForSort(value);
    }

    // EF Core parameterless constructor
    private ModelNumber() { }

    /// <summary>
    /// Parses the model number into a prefix and numeric part for sorting.
    /// </summary>
    /// <param name="input">The model number string.</param>
    /// <returns>A tuple containing the prefix and numeric part for sorting.</returns>
    private static (string? prefix, int? number) ParseForSort(string input)
    {
        if (string.IsNullOrEmpty(input)) return (null, null);

        var span = input.AsSpan();
        int i = 0;
        while (i < span.Length && !char.IsDigit(span[i])) i++;
        var prefix = i == 0 ? null : span[..i].ToString();

        int start = i;
        while (i < span.Length && char.IsDigit(span[i])) i++;

        if (start < i && int.TryParse(span[start..i].ToString(), out var num))
        {
            return (prefix, num);
        }
        return (prefix, null);
    }
}
