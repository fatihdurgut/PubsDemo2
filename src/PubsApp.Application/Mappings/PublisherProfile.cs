using AutoMapper;
using PubsApp.Application.DTOs;
using PubsApp.Core.Entities;

namespace PubsApp.Application.Mappings;

/// <summary>
/// AutoMapper profile for Publisher entity mappings
/// </summary>
public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        // Entity to DTO
        CreateMap<Publisher, PublisherDto>()
            .ForMember(dest => dest.Titles, opt => opt.MapFrom(src =>
                src.Titles.Select(t => new TitleSummaryDto
                {
                    TitleId = t.TitleId,
                    TitleName = t.TitleName,
                    Type = t.Type,
                    Price = t.Price,
                    PublishedDate = t.PublishedDate
                })));

        CreateMap<Publisher, PublisherSummaryDto>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => $"{src.City}, {src.State}"));

        // DTO to Entity
        CreateMap<CreatePublisherDto, Publisher>();

        CreateMap<UpdatePublisherDto, Publisher>()
            .ForMember(dest => dest.PublisherId, opt => opt.Ignore()) // Don't update ID
            .ForMember(dest => dest.Titles, opt => opt.Ignore()); // Don't update navigation
    }
}
