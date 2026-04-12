using AutoMapper;
using Ration.Model;

namespace Ration.Mappers;

public class GetAllBasalRationsMapper : Profile
{
    public GetAllBasalRationsMapper()
    {
        CreateMap<Domain.Entities.FeedType, RationFeedTypeDTO>();
    }
}