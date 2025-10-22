using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportLink.Domain.Entities;

namespace TransportLink.Infrastructure.Configurations;

public sealed class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("shipments");

        builder.HasKey(shipment => shipment.Id);

        builder.Property(shipment => shipment.Reference)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(shipment => shipment.Origin)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(shipment => shipment.Destination)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(shipment => shipment.Status)
            .IsRequired();

        builder.Property(shipment => shipment.CreatedOn)
            .IsRequired();
    }
}
