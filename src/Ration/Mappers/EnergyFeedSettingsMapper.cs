using AutoMapper;
using Domain.Entities;
using Ration.Model;

namespace Ration.Mappers;

public class EnergyFeedSettingsMapper : Profile
{
    public EnergyFeedSettingsMapper()
    {
        CreateMap<EnergyFeedSettings, EnergyFeedSettingsDto>()
            .ForMember(dest => dest.MaxEnergyFeed, opt => opt.MapFrom(src => src.MaxEnergyFeed))
            .ForMember(dest => dest.MinEnergyFeed, opt => opt.MapFrom(src => src.MinEnergyFeed))
            .ForMember(dest => dest.FeedTypeId, opt => opt.MapFrom(src => src.FeedTypeId))
            .ForMember(dest => dest.FeedTypeName, opt => opt.Ignore()); // Map in the service
    }
}