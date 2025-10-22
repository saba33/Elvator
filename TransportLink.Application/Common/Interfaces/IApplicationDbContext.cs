using Microsoft.EntityFrameworkCore;
using TransportLink.Domain.Entities;

namespace TransportLink.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Shipment> Shipments { get; }

    DbSet<Order> Orders { get; }

    DbSet<Company> Companies { get; }

    DbSet<Driver> Drivers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
