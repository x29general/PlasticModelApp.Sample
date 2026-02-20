using AutoMapper;

namespace PlasticModelApp.Application.Shared;

/// <summary>
/// Defines AutoMapper mappings for the Application layer.
/// </summary>
public class ApplicationMappingProfile : Profile
{
    /// <summary>
    /// Initializes application mapping definitions.
    /// </summary>
    public ApplicationMappingProfile()
    {
        // Application layer has no persistence/entity-to-DTO mapping definitions.
        // Those mappings belong to Infrastructure profiles.
    }
}


