using PlasticModelApp.Domain.SharedKernel.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.ValueObjects;

/// <summary>
/// Price of a product Value Object
/// </summary>
public sealed class Price : ValueObject
{
    /// <summary>
    /// Amount in Japanese Yen
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Represents a Price with an amount of zero.
    /// </summary>
    public static Price Zero => new(0m);

    /// <summary>
    /// Creates a Price from the specified amount.
    /// </summary>
    /// <param name="amount">An amount in Japanese Yen.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when amount is less than 0.</exception>
    public Price(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Price must be greater than or equal to 0.");
 
        // Business rule: normalize price to 2 decimal places.
        Amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Returns the components that define the equality of the value object.
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }   

    private Price() { }
 }
