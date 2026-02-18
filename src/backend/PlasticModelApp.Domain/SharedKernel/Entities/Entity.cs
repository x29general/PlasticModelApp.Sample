namespace PlasticModelApp.Domain.SharedKernel.Entities;

/// <summary>
/// Non-generic base class for all entities.
/// Use this to share behaviors that do not depend on identifier type.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Determines whether this entity has the same identity as another entity.
    /// </summary>
    /// <param name="other">The entity to compare with.</param>
    /// <returns>true when both entities are the same type and have the same identity; otherwise false.</returns>
    public bool IsSameEntityAs(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        return HasSameIdentityAs(other);
    }

    /// <summary>
    /// Determines whether this entity has the same identity as another entity of the same runtime type.
    /// </summary>
    /// <param name="other">The entity to compare with.</param>
    /// <returns>true when identities are equal; otherwise false.</returns>
    protected abstract bool HasSameIdentityAs(Entity other);
}

/// <summary>
/// Base class for entities with a unique identifier of type TId.
/// </summary>
public abstract class Entity<TId> : Entity
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <inheritdoc />
    protected override bool HasSameIdentityAs(Entity other)
    {
        if (other is not Entity<TId> otherEntity)
        {
            return false;
        }

        if (Id is null || otherEntity.Id is null)
        {
            return false;
        }

        return EqualityComparer<TId>.Default.Equals(Id, otherEntity.Id);
    }
}
