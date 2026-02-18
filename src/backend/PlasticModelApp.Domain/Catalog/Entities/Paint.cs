using System;
using PlasticModelApp.Domain.SharedKernel.Entities;
using PlasticModelApp.Domain.SharedKernel.ValueObjects;
using PlasticModelApp.Domain.Catalog.ValueObjects;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// Paint Entity - Represents a paint product in the catalog context.
/// </summary>
public class Paint : Entity<PaintId>, IAggregateRoot
{
    // -------------------------------
    // Basic Information
    // -------------------------------

    /// <summary>
    /// Paint Name
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Model Number
    /// </summary>
    public ModelNumber ModelNumber { get; private set; } = null!;

    /// <summary>
    /// Price
    /// </summary>
    public Price Price { get; private set; } = null!;

    /// <summary>
    /// Color Specification
    /// </summary>
    public ColorSpec Color { get; private set; } = null!;

    /// <summary>
    /// Image URL
    /// </summary>
    public string? ImageUrl { get; private set; }

    // -------------------------------
    // Reference Master
    // -------------------------------

    /// <summary>
    /// Brand ID
    /// </summary>
    public BrandId BrandId { get; private set; } = null!;

    /// <summary>
    /// Paint Type ID
    /// </summary>
    public PaintTypeId PaintTypeId { get; private set; } = null!;

    /// <summary>
    /// Gloss ID
    /// </summary>
    public GlossId GlossId { get; private set; } = null!;

    // -------------------------------
    // Audit Information
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
    /// Logical Deletion Flag
    /// </summary>
    public bool IsDeleted { get; private set; }

    // -------------------------------
    // Constructor / Factory Method
    // -------------------------------
    protected Paint() { }

    /// <summary>
    /// Creates a new Paint instance.
    /// </summary>
    /// <param name="id">Paint ID</param>
    /// <param name="name">Paint Name</param>
    /// <param name="brandId">Brand ID</param>
    /// <param name="paintTypeId">Paint Type ID</param>
    /// <param name="glossId">Gloss ID</param>
    /// <param name="color">Color Specification</param>
    /// <param name="price">Price</param>
    /// <param name="modelNumber">Model Number</param>
    /// <param name="description">Description</param>
    /// <param name="imageUrl">Image URL</param>
    /// <returns>A new Paint instance.</returns>
    /// <exception cref="ArgumentException">Thrown when any of the arguments are invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when any of the required arguments are null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when price is negative.</exception>
    private Paint(
        PaintId id,
        string name,
        BrandId brandId,
        PaintTypeId paintTypeId,
        GlossId glossId,
        ColorSpec color,
        Price price,
        ModelNumber modelNumber,
        DateTimeOffset now,
        string? description = null,
        string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Paint Name is cannot be empty.", nameof(name));
        if (brandId == null)
            throw new ArgumentNullException(nameof(brandId), "Brand Id is cannot be empty.");
        if (paintTypeId == null)
            throw new ArgumentNullException(nameof(paintTypeId), "Paint Type Id is cannot be empty.");
        if (glossId == null)
            throw new ArgumentNullException(nameof(glossId), "Gloss Id is cannot be empty.");
        if (color == null)
            throw new ArgumentNullException(nameof(color), "Color Spec cannot be empty.");
        if (price == null || price.Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price is must be 0 or more.");
        if (modelNumber == null)
            throw new ArgumentNullException(nameof(modelNumber), "Model Number is cannot be empty.");

        Id = id;
        Name = name;
        BrandId = brandId;
        PaintTypeId = paintTypeId;
        GlossId = glossId;
        Color = color;
        Price = price;
        ModelNumber = modelNumber;
        CreatedAt = now;
        UpdatedAt = now;
        Description = description;
        ImageUrl = imageUrl;
    }

    /// <summary>
    /// Creates a new Paint instance with the specified ID.
    /// </summary>
    /// <param name="id">Paint ID</param>
    /// <param name="name">Paint Name</param>
    /// <param name="brandId">Brand ID</param>
    /// <param name="paintTypeId">Paint Type ID</param>
    /// <param name="glossId">Gloss ID</param>
    /// <param name="color">Color Specification</param>
    /// <param name="price">Price</param>
    /// <param name="modelNumber">Model Number</param>
    /// <param name="description">Description</param>
    /// <param name="imageUrl">Image URL</param>
    /// <returns>A new Paint instance.</returns>
    public static Paint Create(
        PaintId id,
        string name,
        BrandId brandId,
        PaintTypeId paintTypeId,
        GlossId glossId,
        ColorSpec color,
        Price price,
        ModelNumber modelNumber,
        DateTimeOffset now,
        string? description = null,
        string? imageUrl = null)
        => new(id, name, brandId, paintTypeId, glossId, color, price, modelNumber, now, description, imageUrl);

    /// <summary>
    /// Creates a new Paint instance with a new ID.
    /// </summary>
    /// <param name="name">Paint Name</param>
    /// <param name="brandId">Brand ID</param>
    /// <param name="paintTypeId">Paint Type ID</param>
    /// <param name="glossId">Gloss ID</param>
    /// <param name="color">Color Specification</param>
    /// <param name="price">Price</param>
    /// <param name="modelNumber">Model Number</param>
    /// <param name="description">Description</param>
    /// <param name="imageUrl">Image URL</param>
    /// <returns>A new Paint instance.</returns>
    public static Paint Create(
        string name,
        BrandId brandId,
        PaintTypeId paintTypeId,
        GlossId glossId,
        ColorSpec color,
        Price price,
        ModelNumber modelNumber,
        DateTimeOffset now,
        string? description = null,
        string? imageUrl = null)
        => new(PaintId.NewId(), name, brandId, paintTypeId, glossId, color, price, modelNumber, now, description, imageUrl);

    // -------------------------------
    // Update Methods
    // -------------------------------

    /// <summary>
    /// Updates the details of the Paint.
    /// </summary>
    /// <param name="name">Paint Name</param>
    /// <param name="description">Description</param>
    /// <param name="modelNumber">Model Number</param>
    /// <param name="price">Price</param>
    /// <param name="imageUrl">Image URL</param>
    /// <exception cref="ArgumentException">Thrown when any of the arguments are invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when any of the required arguments are null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any of the arguments are out of range.</exception>
    public void UpdateDetails(
        string name,
        string? description,
        ModelNumber modelNumber,
        Price price,
        BrandId brandId,
        PaintTypeId paintTypeId,
        GlossId glossId,
        ColorSpec color,
        string? imageUrl,
        DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Paint Name is cannot be empty.", nameof(name));
        if (brandId == null)
            throw new ArgumentNullException(nameof(brandId), "Brand Id is cannot be empty.");
        if (paintTypeId == null)
            throw new ArgumentNullException(nameof(paintTypeId), "Paint Type Id is cannot be empty.");
        if (glossId == null)
            throw new ArgumentNullException(nameof(glossId), "Gloss Id is cannot be empty.");
        if (color == null)
            throw new ArgumentNullException(nameof(color), "Color Spec cannot be empty.");
        if (price == null || price.Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price is must be 0 or more.");
        if (modelNumber == null)
            throw new ArgumentNullException(nameof(modelNumber), "Model Number is cannot be empty.");

        Name = name;
        Description = description;
        ModelNumber = modelNumber;
        Price = price;
        BrandId = brandId;
        PaintTypeId = paintTypeId;
        GlossId = glossId;
        Color = color;
        ImageUrl = imageUrl;
        Touch(now);
    }

    /// <summary>
    /// Marks the Paint as deleted.
    /// </summary>
    public void MarkAsDeleted(DateTimeOffset now)
    {
        IsDeleted = true;
        Touch(now);
    }

    /// <summary>
    /// Updates the UpdatedAt timestamp.
    /// </summary>
    /// <param name="now">The current date and time.</param>
    private void Touch(DateTimeOffset now) => UpdatedAt = now;
}
