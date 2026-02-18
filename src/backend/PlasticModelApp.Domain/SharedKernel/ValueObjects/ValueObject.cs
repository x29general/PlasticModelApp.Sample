namespace PlasticModelApp.Domain.SharedKernel.ValueObjects;

/// <summary>
/// Base class for value objects.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>true if the specified object is equal to the current value object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        var thisValues = GetEqualityComponents().GetEnumerator();
        var otherValues = other.GetEqualityComponents().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (thisValues.Current is null ^ otherValues.Current is null) return false;
            if (thisValues.Current is not null && !thisValues.Current.Equals(otherValues.Current)) return false;
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    /// <summary>
    /// Provides a default hash function.
    /// </summary>
    /// <returns>The hash code for the current value object.</returns>
    public override int GetHashCode()
    {
        var hash = new System.HashCode();
        hash.Add(GetType());

        foreach (var component in GetEqualityComponents())
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }

    /// <summary>
    /// Returns the components that define equality for this value object.
    /// </summary>
    /// <returns>An IEnumerable that enumerates the components of the value object.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();
}
