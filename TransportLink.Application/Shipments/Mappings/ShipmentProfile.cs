using AutoMapper;
using TransportLink.Application.Shipments.DTOs;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Shipments.Mappings;

public sealed class ShipmentProfile : Profile
{
    public ShipmentProfile()
    {
        CreateMap<Shipment, ShipmentDto>();
    }
}
