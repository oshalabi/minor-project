using AutoMapper;
using BasalRation.Events.Ration;
using Domain.Entities;
using Microsoft.Extensions.Options;

namespace BasalRation.Mappers;

public class CreateRationMapper : Profile
{
    public CreateRationMapper()
    {
        CreateMap<RationCreated, Ration>();
    }
}