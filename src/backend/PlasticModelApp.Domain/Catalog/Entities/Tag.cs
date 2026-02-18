using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.Entities;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// Tag Entity represents a tag used for categorizing and describing paints in the system.
/// Used for features like genre, usage, and special effects.
/// </summary>
public class Tag : Entity<TagId>, IAggregateRoot
{
    // -------------------------------
    // Basic Information
    // -------------------------------

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// HEX Color Code
    /// </summary>
    public string? Hex { get; private set; }

    /// <summary>
    /// Effect (e.g., Metallic, Clear)
    /// </summary>
    public string? Effect { get; private set; }

    // -------------------------------
    // Foreign Keys / Navigation Properties
    // -------------------------------

    /// <summary>
    /// Tag Category ID
    /// </summary>
    public TagCategoryId TagCategoryId { get; private set; } = null!;
 
    // -------------------------------
    // Audit Fields
    // -------------------------------
 
    /// <summary>
    /// Created At
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
 
    /// <summary>
    /// Updated At
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// Deletion Flag (Soft Delete)
    /// </summary>
    public bool IsDeleted { get; private set; } = false;

    // -------------------------------
    // Constructor / Factory Methods
    // -------------------------------
    protected Tag() { }

    /// <summary>
    /// Initializes a new instance of the Tag class.
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <param name="name">Tag Name</param>
    /// <param name="tagCategory">Tag Category</param>
    /// <param name="description">Description (Optional)</param>
    /// <param name="hex">HEX Color Code (Optional)</param>
    /// <param name="effect">Effect (Optional)</param>
    /// <returns>A new Tag instance.</returns>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
    private Tag(
        string id,
        string name,
        TagCategoryId tagCategory,
        DateTimeOffset now,
        string? description = null,
        string? hex = null,
        string? effect = null)
    {
        ArgumentNullException.ThrowIfNull(tagCategory);

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tag Name is cannot be empty.", nameof(name));
        if (hex != null && !IsValidHex(hex))
            throw new ArgumentException("Invalid HEX code.", nameof(hex));

        Id = TagId.From(id);
        Name = name;
        TagCategoryId = tagCategory;
        Description = description;
        Hex = hex;
        Effect = effect;
        CreatedAt = now;
        UpdatedAt = now;
        IsDeleted = false;
    }

    /// <summary>
    /// Initializes a new instance of the Tag class.
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <param name="name">Tag Name</param>
    /// <param name="tagCategory">Tag Category</param>
    /// <param name="description">Description (Optional)</param>
    /// <param name="hex">HEX Color Code (Optional)</param>
    /// <param name="effect">Effect (Optional)</param>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
    /// <returns>A new Tag instance.</returns>
    public static Tag Create(
        string id,
        string name,
        TagCategoryId tagCategory,
        DateTimeOffset now,
        string? description = null,
        string? hex = null,
        string? effect = null)
        => new Tag(id, name, tagCategory, now, description, hex, effect);
 
    // -------------------------------
    // Domain Methods
    // -------------------------------
 
    /// <summary>
    /// Updates the details of the tag.
    /// </summary>
    /// <param name="name">Tag Name</param>
    /// <param name="tagCategory">Tag Category</param>
    /// <param name="description">Description (Optional)</param>
    /// <param name="hex">HEX Color Code (Optional)</param>
    /// <param name="effect">Effect (Optional)</param>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace, or when HEX is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when attempting to update a deleted tag.</exception>
    public void Update(
        string name,
        TagCategoryId tagCategory,
        DateTimeOffset now,
        string? description = null,
        string? hex = null,
        string? effect = null)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted tag.");

        ArgumentNullException.ThrowIfNull(tagCategory);

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tag Name is cannot be empty.", nameof(name));
 
        if (hex != null && !IsValidHex(hex))
            throw new ArgumentException("Invalid HEX code.", nameof(hex));
 
        Name = name;
        TagCategoryId = tagCategory;
        Description = description;
        Hex = hex;
        Effect = effect;
        Touch(now);
    }
 
    /// <summary>
    /// Marks the tag as deleted (soft delete).
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to delete an already deleted tag.</exception>
    public void MarkAsDeleted(DateTimeOffset now)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Tag is already deleted.");
 
        IsDeleted = true;
        Touch(now);
    }

    /// <summary>
    /// Validates the HEX color code format.
    /// </summary>
    /// <param name="hex">HEX string</param>
    /// <returns>Returns true if valid, false otherwise.</returns>
    private static bool IsValidHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) return false;
        if (hex.Length != 7 || hex[0] != '#') return false;
        return hex.Skip(1).All(c => "0123456789ABCDEFabcdef".Contains(c));
    }

    /// <summary>
    /// Updates the UpdatedAt timestamp.
    /// </summary>
    /// <param name="now">The current date and time.</param>
    private void Touch(DateTimeOffset now) => UpdatedAt = now;
 }
