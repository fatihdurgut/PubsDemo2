using AutoMapper;
using PubsApp.Application.DTOs;
using PubsApp.Core.Entities;

namespace PubsApp.Application.Mappings;

/// <summary>
/// AutoMapper profile for Author entity mappings
/// </summary>
public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        // Entity to DTO
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.Titles, opt => opt.MapFrom(src =>
                src.TitleAuthors.Select(ta => new TitleSummaryDto
                {
                    TitleId = ta.Title.TitleId,
                    TitleName = ta.Title.TitleName,
                    Type = ta.Title.Type,
                    Price = ta.Title.Price,
                    PublishedDate = ta.Title.PublishedDate
                })));

        CreateMap<Author, AuthorSummaryDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.AuthorOrder, opt => opt.Ignore())
            .ForMember(dest => dest.RoyaltyPercentage, opt => opt.Ignore());

        // DTO to Entity
        CreateMap<CreateAuthorDto, Author>();

        CreateMap<UpdateAuthorDto, Author>()
            .ForMember(dest => dest.AuthorId, opt => opt.Ignore()) // Don't update ID
            .ForMember(dest => dest.TitleAuthors, opt => opt.Ignore()); // Don't update navigation
    }
}
