using AutoMapper;
using Domain.Entities;
using Ration.Commands;

namespace Ration.Mappers;

public class LivestockPropertyMapper : Profile
{
    public LivestockPropertyMapper()
    {
        CreateMap<UpdateLivestockProperties, LivestockProperties>();
    }
    
}