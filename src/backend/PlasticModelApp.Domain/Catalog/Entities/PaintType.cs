using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.Entities;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// PaintType represents different types of paint in the system.
/// PaintType: Acrylic, Enamel, Lacquer, etc.
/// </summary>
public class PaintType : Entity<PaintTypeId>
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
     protected PaintType() { }
 
     /// <summary>
     /// Initializes a new instance of the PaintType class.
     /// </summary>
     /// <param name="name">Paint Type Name</param>
     /// <param name="description">Optional description</param>
     /// <returns>A new PaintType instance.</returns>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     private PaintType(string id, string name, string? description = null)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Paint Type Name is cannot be empty.", nameof(name));
 
         Id = PaintTypeId.From(id);
         Name = name;
         Description = description;
     }

    /// <summary>
    /// Initializes a new instance of the PaintType class.
    /// </summary>
    /// <param name="name">Paint Type Name</param>
    /// <param name="description">Optional description</param>
    /// <returns>A new PaintType instance.</returns>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
    public static PaintType Create(
         string id,
         string name,
         string? description = null)
         => new PaintType(id, name, description);
 
     // -------------------------------
     // Domain Methods
     // -------------------------------
 
     /// <summary>
     /// Updates the paint type entity. 
     /// </summary>
     /// <param name="name">New paint type name</param>
     /// <param name="description">New description</param>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     public void Update(string name, string? description)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Paint Type Name is cannot be empty.", nameof(name));
 
         Name = name;
         Description = description;
     }
 }
 
