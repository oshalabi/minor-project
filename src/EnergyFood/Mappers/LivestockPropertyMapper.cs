using EnergyFood.Model;
using AutoMapper;
using Domain.Entities;

namespace EnergyFood.Mappers;

public class LivestockPropertyMapper : Profile
{
    public LivestockPropertyMapper()
    {
        CreateMap<LivestockPropertyDTO, LivestockProperties>();
    }
}