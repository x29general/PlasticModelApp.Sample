namespace PlasticModelApp.Domain.SharedKernel.ValueObjects;

/// <summary>
/// Identifier value object for User.
/// </summary>
public sealed class UserId : ValueObject
{
    /// <summary>
    /// Identifier value.
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    /// <summary>
    /// Create a new random identifier.
    /// </summary>
    public static UserId NewId() => new(Guid.NewGuid().ToString());

    /// <summary>
    /// Create from a specified value.
    /// </summary>
    /// <param name="value">Identifier value.</param>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace.</exception>
    public UserId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("UserId cannot be empty.", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Create from a specified value.
    /// </summary>
    public static UserId From(string value) => new(value);

    /// <summary>
    /// Convert to string.
    /// </summary>
    public override string ToString() => Value;

    /// <summary>
    /// Components used for equality.
    /// </summary>
    /// <returns>Sequence of equality components.</returns>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
