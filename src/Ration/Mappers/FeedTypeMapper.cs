using AutoMapper;
using Domain.Entities;
using Ration.Commands;
using Ration.Model;

namespace Ration.Mappers;

public class FeedTypeMapper : Profile
{
    public FeedTypeMapper()
    {
        CreateMap<FeedTypeDTO, FeedType>();
        CreateMap<NutrientDTO, Nutrient>();
        CreateMap<NutrientTypeDTO, NutrientType>()
            .ForMember(x => x.Value, opts => opts.MapFrom(x => (NutrientTypeValue)x.Value));
    }
}