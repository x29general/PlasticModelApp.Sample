using AutoMapper;
//using PlasticModelApp.Application.CompareLists.Queries;
using PlasticModelApp.Application.Tags.Queries;
using PlasticModelApp.Application.Masters.Queries;
using PlasticModelApp.Domain.Catalog.Entities;
//using CompareListEntity = PlasticModelApp.Domain.Selection.Entities.CompareList;

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
        // Tags: Tag entity -> query DTO.
        CreateMap<Tag, GetTagByIdResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.TagCategoryId.Value));

        // Users: User entity -> query DTO.
        // CreateMap<User, GetUserByIdResult>()
        //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

        // Brand: Brand entity -> query DTO.
        CreateMap<Brand, GetBrandResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

        // Gloss: Gloss entity -> query DTO.
        CreateMap<Gloss, GetGlossResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

        // Paint type: PaintType entity -> query DTO.
        CreateMap<PaintType, GetPaintTypeResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

        // Tag category: TagCategory entity -> query DTO.
        CreateMap<TagCategory, GetTagCategoryResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

        // CompareList
        // CreateMap<CompareListEntity, MyCompareListSummaryResult>()
        //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        // Paint notes: domain entities -> query DTOs.
        // CreateMap<PaintNote, GetPaintNoteResult>()
        //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
        //     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Value))
        //     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.DisplayName))
        //     .ForMember(dest => dest.PaintId, opt => opt.MapFrom(src => src.PaintId.Value))
        //     .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

        // CreateMap<NoteImage, GetPaintNoteImageResult>()
        //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
        //     .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
        //     .ForMember(dest => dest.Caption, opt => opt.MapFrom(src => src.Caption))
        //     .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
        //     .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}


