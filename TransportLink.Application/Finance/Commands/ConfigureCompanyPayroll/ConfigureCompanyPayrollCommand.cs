using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Application.Finance.DTOs;
using TransportLink.Domain.Common;
using TransportLink.Domain.Entities;
using TransportLink.Domain.Enums;

namespace TransportLink.Application.Finance.Commands.ConfigureCompanyPayroll;

public sealed record ConfigureCompanyPayrollCommand(
    Guid CompanyId,
    SalaryMode SalaryMode,
    decimal? PerOrderRatePct,
    decimal? DailyRate,
    decimal? MonthlySalary) : IRequest<Result<CompanyPayrollConfigDto>>;

public sealed class ConfigureCompanyPayrollCommandHandler : IRequestHandler<ConfigureCompanyPayrollCommand, Result<CompanyPayrollConfigDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ConfigureCompanyPayrollCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<CompanyPayrollConfigDto>> Handle(ConfigureCompanyPayrollCommand request, CancellationToken cancellationToken)
    {
        var companyExists = await _context.Companies
            .AsNoTracking()
            .AnyAsync(company => company.Id == request.CompanyId, cancellationToken);

        if (!companyExists)
        {
            return Result<CompanyPayrollConfigDto>.Failure("Company not found.");
        }

        var configuration = await _context.CompanyPayrollConfigs
            .FirstOrDefaultAsync(config => config.CompanyId == request.CompanyId, cancellationToken);

        if (configuration is null)
        {
            configuration = new CompanyPayrollConfig(
                request.CompanyId,
                request.SalaryMode,
                request.PerOrderRatePct,
                request.DailyRate,
                request.MonthlySalary,
                DateTime.UtcNow);

            await _context.CompanyPayrollConfigs.AddAsync(configuration, cancellationToken);
        }
        else
        {
            configuration.UpdateConfiguration(
                request.SalaryMode,
                request.PerOrderRatePct,
                request.DailyRate,
                request.MonthlySalary);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<CompanyPayrollConfigDto>.Success(_mapper.Map<CompanyPayrollConfigDto>(configuration));
    }
}
