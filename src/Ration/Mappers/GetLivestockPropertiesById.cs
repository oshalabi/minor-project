using AutoMapper;
using Domain.Entities;
using Ration.Model;

namespace Ration.Mappers;

public class GetLivestockPropertiesById : Profile
{
    public GetLivestockPropertiesById()
    {
        CreateMap<LivestockProperties, LivestockPropertiesDTO>();
    }
}