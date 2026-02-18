using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.Entities;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// Gloss Entity - Represents a gloss type of paints in the system.
/// </summary>
public class Gloss : Entity<GlossId>
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
     protected Gloss() { }
 
     /// <summary>
     /// Creates a new instance of the Gloss class.
     /// </summary>
     /// <param name="name">Gloss Name</param>
     /// <param name="description">Optional description of the gloss</param>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     private Gloss(string id, string name, string? description = null)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Gloss Name is cannot be empty.", nameof(name));
 
         Id = GlossId.From(id);
         Name = name;
         Description = description;
     }

    /// <summary>
    /// Creates a new instance of the Gloss class.
    /// </summary>
    /// <param name="name">Gloss Name</param>
    /// <param name="description">Optional description of the gloss</param>
    /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
    public static Gloss Create(
         string id,
         string name,
         string? description = null)
         => new Gloss(id, name, description);
 
     // -------------------------------
     // Domain Methods
     // -------------------------------
 
     /// <summary>
     /// Updates the gloss entity.
     /// </summary>
     /// <param name="name">New Gloss Name</param>
     /// <param name="description">New Description</param>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     public void Update(string name, string? description)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Gloss Name is cannot be empty.", nameof(name));
 
         Name = name;
         Description = description;
     }
 }
