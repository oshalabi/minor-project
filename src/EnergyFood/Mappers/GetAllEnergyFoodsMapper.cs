using EnergyFood.Model;
using AutoMapper;

namespace EnergyFood.Mappers;

public class GetAllEnergyFoodsMapper : Profile
{
    public GetAllEnergyFoodsMapper()
    {
        CreateMap<Domain.Entities.FeedType, FeedTypeDTO>();
    }
}