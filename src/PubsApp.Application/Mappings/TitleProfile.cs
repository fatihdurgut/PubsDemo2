using AutoMapper;
using PubsApp.Application.DTOs;
using PubsApp.Core.Entities;

namespace PubsApp.Application.Mappings;

/// <summary>
/// AutoMapper profile for Title entity mappings
/// </summary>
public class TitleProfile : Profile
{
    public TitleProfile()
    {
        // Entity to DTO
        CreateMap<Title, TitleDto>()
            .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher != null
                ? new PublisherSummaryDto
                {
                    PublisherId = src.Publisher.PublisherId,
                    PublisherName = src.Publisher.PublisherName,
                    Location = $"{src.Publisher.City}, {src.Publisher.State}"
                }
                : null))
            .ForMember(dest => dest.Authors, opt => opt.MapFrom(src =>
                src.TitleAuthors.Select(ta => new AuthorSummaryDto
                {
                    AuthorId = ta.Author.AuthorId,
                    FullName = $"{ta.Author.FirstName} {ta.Author.LastName}",
                    AuthorOrder = ta.AuthorOrder,
                    RoyaltyPercentage = ta.RoyaltyPercentage
                })));

        CreateMap<Title, TitleSummaryDto>();

        // DTO to Entity
        CreateMap<CreateTitleDto, Title>();

        CreateMap<UpdateTitleDto, Title>()
            .ForMember(dest => dest.TitleId, opt => opt.Ignore()) // Don't update ID
            .ForMember(dest => dest.Publisher, opt => opt.Ignore()) // Don't update navigation
            .ForMember(dest => dest.TitleAuthors, opt => opt.Ignore()); // Don't update navigation
    }
}
