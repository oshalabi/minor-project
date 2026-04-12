using AutoMapper;
using Ration.Commands;

namespace Ration.Mappers;

public class CreateRationMapper : Profile
{
    public CreateRationMapper()
    {
        CreateMap<CreateRation, Domain.Entities.Ration>();
    }
}