using AutoMapper;
using TransportLink.Application.Finance.DTOs;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Finance.Mappings;

public sealed class FinanceProfile : Profile
{
    public FinanceProfile()
    {
        CreateMap<Payment, PaymentDto>();
        CreateMap<CompanyPayrollConfig, CompanyPayrollConfigDto>();
    }
}
