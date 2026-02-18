using PlasticModelApp.Domain.Catalog.ValueObjects;
using PlasticModelApp.Domain.SharedKernel.Entities;

namespace PlasticModelApp.Domain.Catalog.Entities;

/// <summary>
/// TagCategory Entity represents a category for tags used in the system.
/// </summary>
public class TagCategory : Entity<TagCategoryId>
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
     protected TagCategory() { }
 
     /// <summary>
     /// Initializes a new instance of the TagCategory class.
     /// </summary>
     /// <param name="id">Tag Category ID</param>
     /// <param name="name">Tag Category Name</param>
     /// <param name="description">Description (Optional)</param>
     /// <returns>A new TagCategory instance.</returns>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     private TagCategory(string id, string name, string? description = null)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Tag Category Name is cannot be empty.", nameof(name));
 
         Id = TagCategoryId.From(id);
         Name = name;
         Description = description;
     }
 
     /// <summary>
     /// Initializes a new instance of the TagCategory class.
     /// </summary>
     /// <param name="id">Tag Category ID</param>
     /// <param name="name">Tag Category Name</param>
     /// <param name="description">Description (Optional)</param>
     /// <returns>A new TagCategory instance.</returns>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>

     public static TagCategory Create(
         string id,
         string name,
         string? description = null)
         => new TagCategory(id, name, description);
     
     // -------------------------------
     // Domain Methods
     // -------------------------------
 
     /// <summary>
     /// Updates the details of the tag category.
     /// </summary>
     /// <param name="name">Tag Category Name</param>
     /// <param name="description">Description (Optional)</param>
     /// <exception cref="ArgumentException">Thrown when name is null, empty, or whitespace.</exception>
     public void Update(string name, string? description)
     {
         if (string.IsNullOrWhiteSpace(name))
             throw new ArgumentException("Tag Category Name is cannot be empty.", nameof(name));
 
         Name = name;
         Description = description;
     }
}
