namespace PlasticModelApp.Domain.SharedKernel.ValueObjects;

/// <summary>
/// Identifier for Paint entity.
/// </summary>
public sealed class PaintId : ValueObject
{
    /// <summary>
    /// The value of the identifier.
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Generates a new identifier value.
    /// </summary>
    public static PaintId NewId() => new(Guid.NewGuid().ToString());

    /// <summary>
    /// Returns the string representation of this identifier.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Returns the components that define equality for this value object.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaintId"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the identifier.</param>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace.</exception>
    public PaintId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PaintId cannot be empty.", nameof(value));

        Value = value;
    }
}
