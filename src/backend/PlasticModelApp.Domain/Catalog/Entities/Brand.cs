using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.Entities;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// Brand Entity - Represents a brand of paints in the catalog context.
/// </summary>
public class Brand : Entity<BrandId>
 {
     // -------------------------------
     // Basic Information
     // -------------------------------
 
     /// <summary>
     /// Name
     /// </summary>
     public string Name { get; private set; } = string.Empty;
 
     /// <summary>
     /// Description
     /// </summary>
     public string? Description { get; private set; }
 
     // -------------------------------
     // Constructor / Factory Methods
     // -------------------------------
     protected Brand() { }
 
     /// <summary>
     /// Creates a new Brand instance.
     /// </summary>
     /// <param name="name">Brand Name</param>
     /// <param name="description">Description</param>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     private Brand(string id, string name, string? description = null)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Brand Name is cannot be empty.", nameof(name));
 
         Id = BrandId.From(id);
         Name = name;
         Description = description;
     }

    /// <summary>
    /// Creates a new Brand instance.
    /// </summary>
    /// <param name="id">Brand ID</param>
    /// <param name="name">Brand Name</param>
    /// <param name="description">Description</param>
    /// <returns>A new Brand instance.</returns>
    public static Brand Create(
         string id,
         string name,
         string? description = null)
         => new Brand(id, name, description);

    // -------------------------------
    // Domain Methods
    // -------------------------------

    /// <summary>
    /// Updates the brand's name and description.
    /// </summary>
    /// <param name="name">New Brand Name</param>
    /// <param name="description">New Description</param>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brand Name is cannot be empty.", nameof(name));
 
         Name = name;
         Description = description;
     }
 }
