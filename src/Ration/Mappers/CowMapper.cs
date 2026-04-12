using AutoMapper;
using Domain.Entities;
using Ration.Model;

namespace Ration.Mappers;

public class CowMapper : Profile
{
    public CowMapper()
    {
        CreateMap<Cow, CowGroupingDTO>();
        CreateMap<Advice, AdviceDTO>();
    }
}