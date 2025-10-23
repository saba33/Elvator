using AutoMapper;
using TransportLink.Application.Drivers.DTOs;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Drivers.Mappings;

public sealed class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<Driver, DriverDto>();
    }
}
