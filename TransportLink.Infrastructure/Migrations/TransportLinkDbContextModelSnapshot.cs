using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TransportLink.Infrastructure.Persistence;

#nullable disable

namespace TransportLink.Infrastructure.Migrations
{
    [DbContext(typeof(TransportLinkDbContext))]
    partial class TransportLinkDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TransportLink.Domain.Entities.Company", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid");

                b.Property<string>("Address")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("ContactEmail")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("character varying(200)");

                b.HasKey("Id");

                b.ToTable("companies");

                b.HasData(
                    new
                    {
                        Id = new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"),
                        Address = "100 Market Street, Springfield",
                        ContactEmail = "contact@logisticshub.com",
                        Name = "Logistics Hub"
                    },
                    new
                    {
                        Id = new Guid("c31a68a0-8ff1-4020-8bd2-1651a8651adb"),
                        Address = "200 Cargo Road, Metropolis",
                        ContactEmail = "ops@expressfreight.io",
                        Name = "Express Freight"
                    });
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Driver", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid");

                b.Property<int>("ActiveAssignments")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasDefaultValue(0);

                b.Property<Guid>("CompanyId")
                    .HasColumnType("uuid");

                b.Property<bool>("IsAvailable")
                    .HasColumnType("boolean");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("character varying(200)");

                b.Property<string>("Phone")
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnType("character varying(32)");

                b.Property<decimal>("Rating")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("numeric(3,2)")
                    .HasDefaultValue(0m);

                b.Property<int>("VehicleType")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("CompanyId");

                b.ToTable("drivers");

                b.HasData(
                    new
                    {
                        Id = new Guid("3c7d5a4f-54b4-4f48-8b1e-7058ff5a9c11"),
                        ActiveAssignments = 0,
                        CompanyId = new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"),
                        IsAvailable = true,
                        Name = "Alice Walker",
                        Phone = "+1-555-0101",
                        Rating = 4.8m,
                        VehicleType = 1
                    },
                    new
                    {
                        Id = new Guid("5ab7136f-0c1a-4b60-8d13-59c9c7bd08bf"),
                        ActiveAssignments = 0,
                        CompanyId = new Guid("4b1a0f97-98f5-4d4d-9e9c-948d1edb5b70"),
                        IsAvailable = true,
                        Name = "Bob Nguyen",
                        Phone = "+1-555-0102",
                        Rating = 4.5m,
                        VehicleType = 0
                    },
                    new
                    {
                        Id = new Guid("f47b4b82-7fb3-4c36-9e39-2adce37b329e"),
                        ActiveAssignments = 0,
                        CompanyId = new Guid("c31a68a0-8ff1-4020-8bd2-1651a8651adb"),
                        IsAvailable = true,
                        Name = "Carlos Diaz",
                        Phone = "+1-555-0103",
                        Rating = 4.9m,
                        VehicleType = 2
                    });
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Order", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid");

                b.Property<string>("CargoType")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("character varying(128)");

                b.Property<Guid>("CompanyId")
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone");

                b.Property<Guid?>("DriverId")
                    .HasColumnType("uuid");

                b.Property<decimal>("Price")
                    .HasColumnType("numeric(18,2)");

                b.Property<int>("Status")
                    .HasColumnType("integer");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp with time zone");

                b.Property<decimal>("WeightKg")
                    .HasColumnType("numeric(18,2)");

                b.HasKey("Id");

                b.HasIndex("DriverId");

                b.ToTable("orders");
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Shipment", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedOn")
                    .HasColumnType("timestamp with time zone");

                b.Property<string>("Destination")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("character varying(128)");

                b.Property<string>("Origin")
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnType("character varying(128)");

                b.Property<string>("Reference")
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnType("character varying(64)");

                b.Property<int>("Status")
                    .HasColumnType("integer");

                b.Property<DateTime?>("UpdatedOn")
                    .HasColumnType("timestamp with time zone");

                b.HasKey("Id");

                b.ToTable("shipments");
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Order", b =>
            {
                b.HasOne("TransportLink.Domain.Entities.Driver", "Driver")
                    .WithMany("Orders")
                    .HasForeignKey("DriverId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("Driver");
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Driver", b =>
            {
                b.HasOne("TransportLink.Domain.Entities.Company", "Company")
                    .WithMany("Drivers")
                    .HasForeignKey("CompanyId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Company");
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Company", b =>
            {
                b.Navigation("Drivers");
            });

            modelBuilder.Entity("TransportLink.Domain.Entities.Driver", b =>
            {
                b.Navigation("Orders");
            });
#pragma warning restore 612, 618
        }
    }
}
