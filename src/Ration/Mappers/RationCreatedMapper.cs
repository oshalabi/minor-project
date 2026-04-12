using AutoMapper;
using Ration.Commands;
using Ration.Model;

namespace Ration.Mappers;

public class RationCreatedMapper : Profile
{
    public RationCreatedMapper()
    {
        CreateMap<Domain.Entities.Ration, RationDTO>();
    }
}