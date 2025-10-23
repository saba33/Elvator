using Microsoft.EntityFrameworkCore;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Shipment> Shipments { get; }

    DbSet<Order> Orders { get; }

    DbSet<Company> Companies { get; }

    DbSet<Driver> Drivers { get; }

    DbSet<Payment> Payments { get; }

    DbSet<CompanyPayrollConfig> CompanyPayrollConfigs { get; }

    DbSet<FinancialTransaction> FinancialTransactions { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
