using AutoMapper;
using TransportLink.Application.Orders.DTOs;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Orders.Mappings;

public sealed class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.Name : null))
            .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.VehicleType : null));
    }
}
