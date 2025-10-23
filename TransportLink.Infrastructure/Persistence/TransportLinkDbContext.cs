using Microsoft.EntityFrameworkCore;
using TransportLink.Application.Common.Interfaces;
using TransportLink.Domain.Entities;

namespace TransportLink.Infrastructure.Persistence;

public sealed class TransportLinkDbContext : DbContext, IApplicationDbContext
{
    public TransportLinkDbContext(DbContextOptions<TransportLinkDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<Driver> Drivers => Set<Driver>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<CompanyPayrollConfig> CompanyPayrollConfigs => Set<CompanyPayrollConfig>();

    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransportLinkDbContext).Assembly);
    }
}
